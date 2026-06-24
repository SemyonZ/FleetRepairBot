using System;

namespace FleetRepairBot.Telegram
{
    public class TelegramBotOptions
    {
        // Bot token for Telegram API. If empty, the hosted service will not start the bot.
        public string BotToken { get; set; } = string.Empty;

        // Optional: long Polling timeout, etc.
        public int PollingTimeoutSeconds { get; set; } = 30;

        // Optional default admin/chat id - not required
        public long? AdminChatId { get; set; }
    }
}
