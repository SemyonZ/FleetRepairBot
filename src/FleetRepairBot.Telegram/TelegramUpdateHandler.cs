using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FleetRepairBot.Telegram
{
    // A simple update handler that replies to basic commands and logs updates.
    // It intentionally keeps domain interactions minimal (it can be extended to use RepairRequest services).
    public class TelegramUpdateHandler
    {
        private readonly ILogger<TelegramUpdateHandler> _logger;
        private readonly TelegramBotOptions _options;

        public TelegramUpdateHandler(ILogger<TelegramUpdateHandler> logger, Microsoft.Extensions.Options.IOptions<TelegramBotOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new TelegramBotOptions();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                if (update == null) return;

                switch (update.Type)
                {
                    case UpdateType.Message:
                        var msg = update.Message;
                        if (msg?.Type == MessageType.Text)
                        {
                            var text = msg.Text.Trim();
                            _logger.LogInformation("Received message from {ChatId}: {Text}", msg.Chat.Id, text);

                            if (text.StartsWith("/start", StringComparison.OrdinalIgnoreCase))
                            {
                                await botClient.SendTextMessageAsync(msg.Chat.Id, "Welcome to FleetRepairBot. Send /new <description> to create a repair request.", cancellationToken: cancellationToken);
                                return;
                            }

                            if (text.StartsWith("/new", StringComparison.OrdinalIgnoreCase))
                            {
                                // Minimal behavior: acknowledge. Integration with RepairRequest service can be added later.
                                var desc = text.Length > 4 ? text.Substring(4).Trim() : "(no description)";
                                var reply = $"Repair request received: {desc}";
                                await botClient.SendTextMessageAsync(msg.Chat.Id, reply, cancellationToken: cancellationToken);
                                _logger.LogInformation("Acknowledged new repair request from {ChatId}", msg.Chat.Id);
                                return;
                            }

                            if (text.StartsWith("/help", StringComparison.OrdinalIgnoreCase))
                            {
                                await botClient.SendTextMessageAsync(msg.Chat.Id, "Commands:\n/start - welcome\n/new <description> - create repair request\n/help - this help", cancellationToken: cancellationToken);
                                return;
                            }

                            // Echo fallback
                            await botClient.SendTextMessageAsync(msg.Chat.Id, "I received: " + text, cancellationToken: cancellationToken);
                        }
                        break;

                    case UpdateType.EditedMessage:
                    case UpdateType.CallbackQuery:
                    default:
                        _logger.LogDebug("Unhandled update type: {UpdateType}", update.Type);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling update");
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Telegram Bot encountered an error");
            // Don't rethrow — hosting service will continue running
            return Task.CompletedTask;
        }
    }
}
