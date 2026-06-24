using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FleetRepairBot.Telegram
{
    public class BotHostedService : BackgroundService
    {
        private readonly ILogger<BotHostedService> _logger;
        private readonly TelegramBotOptions _options;
        private readonly IServiceProvider _services;

        public BotHostedService(IOptions<TelegramBotOptions> options, IServiceProvider services, ILogger<BotHostedService> logger)
        {
            _options = options.Value;
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BotHostedService starting");
            // This is a stub loop to represent bot lifecycle; real bot client should be started here.
            while (!stoppingToken.IsCancellationRequested)
            {
                // Create a scope to resolve scoped services (like IRepairRequestService) and the TelegramUpdateHandler safely.
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var handler = scope.ServiceProvider.GetService<TelegramUpdateHandler>();
                        // If a real implementation were present, you could start or poll the bot here using the handler.
                        // For now, just ensure we can resolve it without holding a scoped service in the singleton.
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while resolving TelegramUpdateHandler in BotHostedService loop");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            _logger.LogInformation("BotHostedService stopping");
        }
    }
}
