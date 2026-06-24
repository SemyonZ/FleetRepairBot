using System;

namespace FleetRepairBot.Telegram
{
    public class TelegramBotOptions
    {
        // Token for the Telegram bot. If empty or whitespace, the hosted service will not start the bot.
        public string BotToken { get; set; }

        // Optional admin chat id for notifications (can be null)
        public long? AdminChatId { get; set; }

        // Directory where telegram-related files (e.g. uploaded photos) can be stored
        public string StorageDirectory { get; set; } = "./BotStorage";
    }
}
