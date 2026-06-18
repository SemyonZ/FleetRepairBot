namespace FleetRepairBot.Telegram
{
    /// <summary>
    /// Options required to configure Telegram client behaviour.
    /// Bind from configuration section "TelegramBot" or set environment variable TELEGRAM_BOT_TOKEN.
    /// </summary>
    public class TelegramBotOptions
    {
        /// <summary>
        /// Bot token (required). Can be provided from configuration or environment variable TELEGRAM_BOT_TOKEN.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// If true, webhook mode will be used instead of long polling.
        /// </summary>
        public bool UseWebhook { get; set; } = false;

        /// <summary>
        /// Webhook URL to register when UseWebhook is true.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Polling related options.
        /// </summary>
        public PollingOptions Polling { get; set; } = new PollingOptions();

        /// <summary>
        /// Optional proxy URL for outgoing connections.
        /// </summary>
        public string? ProxyUrl { get; set; }

        /// <summary>
        /// Optional request timeout in seconds for Telegram client HTTP calls. 0 means default.
        /// </summary>
        public int RequestTimeoutSeconds { get; set; } = 0;
    }

    public class PollingOptions
    {
        /// <summary>
        /// Long polling timeout in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 60;

        /// <summary>
        /// Limit of allowed updates per request. 0 means default.
        /// </summary>
        public int AllowedUpdatesLimit { get; set; } = 0;
    }
}
