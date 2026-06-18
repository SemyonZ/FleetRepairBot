using System.Threading.Tasks;

namespace FleetRepairBot.Telegram
{
    public class TelegramUpdateHandler
    {
        private readonly TelegramBotOptions _options;
        public TelegramUpdateHandler(TelegramBotOptions options)
        {
            _options = options;
        }

        // Minimal handler stub. Real update handling should be implemented separately.
        public Task HandleUpdateAsync(object update)
        {
            // no-op placeholder
            return Task.CompletedTask;
        }
    }
}
