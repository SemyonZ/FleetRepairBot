using System;

namespace FleetRepairBot.Telegram
{
    // Configuration options for Telegram bot
    public class TelegramBotOptions
    {
        // Bot token used to initialize ITelegramBotClient. Provide via configuration (e.g. appsettings or environment).
        public string BotToken { get; set; } = string.Empty;

        // Optional webhook URL. If empty, polling may be used by hosted service.
        public string? WebhookUrl { get; set; }

        // Optional: max number of concurrent update handlers
        public int MaxConcurrentHandlers { get; set; } = 1;
    }
}
