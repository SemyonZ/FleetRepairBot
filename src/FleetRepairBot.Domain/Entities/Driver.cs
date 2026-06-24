using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Driver
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public long TelegramId { get; set; }

        // optional: display or notes
        public string Notes { get; set; }
    }
}
