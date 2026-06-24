using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Dispatcher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long TelegramId { get; set; }

        // No Email as required
        public string Notes { get; set; }
    }
}
