using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace FleetRepairBot.Telegram
{
    public class BotHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<BotHostedService> _logger;
        private readonly TelegramUpdateHandler _updateHandler;
        private readonly TelegramBotOptions _options;

        private ITelegramBotClient _client;
        private CancellationTokenSource _cts;
        private bool _started;

        public BotHostedService(ILogger<BotHostedService> logger,
                                TelegramUpdateHandler updateHandler,
                                IOptions<TelegramBotOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _updateHandler = updateHandler ?? throw new ArgumentNullException(nameof(updateHandler));
            _options = options?.Value ?? new TelegramBotOptions();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.BotToken))
            {
                _logger.LogInformation("Telegram Bot token is empty. Bot will not be started. Application continues without bot.");
                _started = false;
                return Task.CompletedTask;
            }

            try
            {
                _logger.LogInformation("Starting Telegram Bot...");
                _client = new TelegramBotClient(_options.BotToken);

                _cts = new CancellationTokenSource();

                // Start receiving updates. We pass the handler delegates implemented in TelegramUpdateHandler.
                var receiverOptions = new ReceiverOptions
                {
                    // Receive all update types
                    AllowedUpdates = Array.Empty<UpdateType>()
                };

                _client.StartReceiving(
                    _updateHandler.HandleUpdateAsync,
                    _updateHandler.HandleErrorAsync,
                    receiverOptions,
                    _cts.Token
                );

                _started = true;
                _logger.LogInformation("Telegram Bot started.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start Telegram Bot. Bot will not run, but application continues.");
                _started = false;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (!_started)
            {
                _logger.LogInformation("BotHostedService: bot was not started; nothing to stop.");
                return Task.CompletedTask;
            }

            try
            {
                _logger.LogInformation("Stopping Telegram Bot...");
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;
                _client = null;
                _started = false;
                _logger.LogInformation("Telegram Bot stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while stopping Telegram Bot");
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            try
            {
                _cts?.Dispose();
            }
            catch { }
        }
    }
}
