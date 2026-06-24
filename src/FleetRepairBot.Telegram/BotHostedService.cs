using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;

namespace FleetRepairBot.Telegram
{
    public class BotHostedService : IHostedService
    {
        private readonly ILogger<BotHostedService> _logger;
        private readonly string _token;
        private ITelegramBotClient _botClient;

        public BotHostedService(ILogger<BotHostedService> logger, string token)
        {
            _logger = logger;
            _token = token;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _botClient = new TelegramBotClient(_token);
                var receiverOptions = new ReceiverOptions();
                _botClient.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start bot");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // If the client supports disposal, dispose here; keep minimal to avoid architectural changes
            return Task.CompletedTask;
        }

        private Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
        {
            // Placeholder - actual logic exists elsewhere; keep signature intact
            return Task.CompletedTask;
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
        {
            var errorMessage = exception switch
            {
                ApiRequestException api => $"Telegram API Error: {api.ErrorCode} - {api.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(errorMessage);
            return Task.CompletedTask;
        }
    }
}
