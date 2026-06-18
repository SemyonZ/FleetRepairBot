using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace FleetRepairBot.Telegram
{
    public class BotHostedService : IHostedService
    {
        private readonly TelegramUpdateHandler _handler;
        public BotHostedService(TelegramUpdateHandler handler)
        {
            _handler = handler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start logic for bot would go here. Kept minimal for audit restoration.
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop logic
            return Task.CompletedTask;
        }
    }
}
