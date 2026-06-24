using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Data;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Services;
using FleetRepairBot.Infrastructure;
using FleetRepairBot.Telegram;
using FleetRepairBot.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FleetRepairBot.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;
                    var connectionString = configuration.GetConnectionString("DefaultConnection");

                    // Register concrete DbContext (existing Data context)
                    services.AddDbContext<FleetRepairDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    // Repositories (Data layer)
                    services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
                    services.AddScoped<IDriverRepository, DriverRepository>();
                    services.AddScoped<IVehicleRepository, VehicleRepository>();

                    // File storage registration - use configured base path or default to "uploads" under content root
                    var basePath = configuration.GetValue<string>("FileStorage:BasePath");
                    if (string.IsNullOrWhiteSpace(basePath))
                    {
                        basePath = Path.Combine(AppContext.BaseDirectory, "uploads");
                    }
                    services.AddSingleton<IFileStorage>(sp => new FileSystemStorage(basePath));

                    // Application-level services & mapping
                    services.AddScoped<FleetRepairBot.Application.Services.RepairRequestService>();
                    services.AddScoped<FleetRepairBot.Application.Interfaces.IRepairRequestService>(sp => sp.GetRequiredService<FleetRepairBot.Application.Services.RepairRequestService>());

                    // Mapping profile - simple manual mapping included in service, but keep placeholder registration
                    services.AddSingleton<FleetRepairBot.Application.Mappings.MappingProfile>();

                    // Telegram options: prefer environment variable TELEGRAM_BOT_TOKEN over config value
                    var telegramOptions = configuration.GetSection("TelegramBot").Get<TelegramBotOptions>() ?? new TelegramBotOptions();

                    var envToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
                    if (!string.IsNullOrWhiteSpace(envToken))
                    {
                        telegramOptions.BotToken = envToken;
                    }

                    services.AddSingleton(telegramOptions);

                    // Register ITelegramClient adapter using Telegram.Bot client inside Program
                    services.AddSingleton<ITelegramClient>(sp =>
                    {
                        var opts = sp.GetRequiredService<TelegramBotOptions>();
                        if (string.IsNullOrWhiteSpace(opts.BotToken))
                        {
                            // return a noop implementation to avoid nulls in dev
                            return new NoOpTelegramClient();
                        }

                        var bot = new TelegramBotClient(opts.BotToken);
                        return new TelegramClientAdapter(bot);
                    });

                    // Register application TelegramUpdateHandler
                    services.AddSingleton<FleetRepairBot.Application.Services.TelegramUpdateHandler>();

                    // Register hosted service that starts Telegram receiving and routes to our application handler
                    services.AddHostedService(sp => new TelegramBackgroundService(
                        sp.GetRequiredService<ITelegramClient>(),
                        sp.GetRequiredService<FleetRepairBot.Application.Services.TelegramUpdateHandler>(),
                        sp.GetRequiredService<ILogger<TelegramBackgroundService>>(),
                        telegramOptions));

                    // Note: we intentionally do not register the old BotHostedService from Telegram project to avoid duplicate receivers.
                })
                .Build();

            host.Run();
        }

        // Minimal adapter implementing our ITelegramClient interface using Telegram.Bot client
        private class TelegramClientAdapter : ITelegramClient
        {
            private readonly ITelegramBotClient _client;

            public TelegramClientAdapter(ITelegramBotClient client)
            {
                _client = client ?? throw new ArgumentNullException(nameof(client));
            }

            public Task SendTextAsync(long chatId, string text, CancellationToken ct = default)
            {
                return _client.SendTextMessageAsync(chatId, text, cancellationToken: ct);
            }

            public async Task<System.IO.Stream> DownloadFileAsync(string fileId, CancellationToken ct = default)
            {
                var file = await _client.GetFileAsync(fileId, ct).ConfigureAwait(false);
                var ms = new System.IO.MemoryStream();
                await _client.DownloadFileAsync(file.FilePath, ms, ct).ConfigureAwait(false);
                ms.Position = 0;
                return ms;
            }

            public Task SendPhotoAsync(long chatId, System.IO.Stream photoStream, string fileName, string caption = null, CancellationToken ct = default)
            {
                photoStream.Position = 0;
                return _client.SendPhotoAsync(chatId, new Telegram.Bot.Types.InputFiles.InputOnlineFile(photoStream, fileName), caption, cancellationToken: ct);
            }
        }

        // No-op implementation for development without token
        private class NoOpTelegramClient : ITelegramClient
        {
            public Task<System.IO.Stream> DownloadFileAsync(string fileId, CancellationToken ct = default)
            {
                return Task.FromResult<System.IO.Stream>(null);
            }

            public Task SendPhotoAsync(long chatId, System.IO.Stream photoStream, string fileName, string caption = null, CancellationToken ct = default)
            {
                return Task.CompletedTask;
            }

            public Task SendTextAsync(long chatId, string text, CancellationToken ct = default)
            {
                return Task.CompletedTask;
            }
        }

        // Background service that starts Telegram long polling and forwards updates to our application handler
        private class TelegramBackgroundService : BackgroundService
        {
            private readonly ITelegramClient _clientAdapter;
            private readonly FleetRepairBot.Application.Services.TelegramUpdateHandler _handler;
            private readonly ILogger<TelegramBackgroundService> _logger;
            private readonly TelegramBotOptions _options;

            public TelegramBackgroundService(ITelegramClient clientAdapter, FleetRepairBot.Application.Services.TelegramUpdateHandler handler, ILogger<TelegramBackgroundService> logger, TelegramBotOptions options)
            {
                _clientAdapter = clientAdapter;
                _handler = handler;
                _logger = logger;
                _options = options;
            }

            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                if (string.IsNullOrWhiteSpace(_options.BotToken))
                {
                    _logger.LogWarning("Telegram token not configured. TelegramBackgroundService will not start.");
                    return Task.CompletedTask;
                }

                var botClient = new TelegramBotClient(_options.BotToken);
                var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<Telegram.Bot.Types.Enums.UpdateType>() };

                botClient.StartReceiving(async (c, update, ct) =>
                {
                    try
                    {
                        await _handler.HandleAsync(c, update, ct).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in application handler while processing update");
                    }
                }, async (c, ex, ct) =>
                {
                    _logger.LogError(ex, "Telegram receive error");
                    await Task.CompletedTask;
                }, receiverOptions, cancellationToken: stoppingToken);

                _logger.LogInformation("TelegramBackgroundService started receiving updates.");

                return Task.CompletedTask;
            }
        }
    }
}
