namespace FleetRepairBot.Telegram
{
    public class TelegramBotOptions
    {
        public string Token { get; set; }
        public long[] AllowedChatIds { get; set; }
        public bool UsePolling { get; set; } = true;
    }
}
