using FleetRepairBot.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FleetRepairBot.Telegram
{
    // Minimal handler stub - integration with real Telegram client is out of scope here
    public class TelegramUpdateHandler
    {
        private readonly IRepairRequestService _requests;
        private readonly ILogger<TelegramUpdateHandler> _logger;

        public TelegramUpdateHandler(IRepairRequestService requests, ILogger<TelegramUpdateHandler> logger)
        {
            _requests = requests;
            _logger = logger;
        }

        // Process a simple text command (placeholder)
        public async Task HandleTextCommandAsync(long chatId, string text)
        {
            _logger.LogInformation("Received command from {chatId}: {text}", chatId, text);
            // example: /new <vehicleId> <description>
            // For brevity, we don't implement parsing here.
            await Task.CompletedTask;
        }
    }
}
