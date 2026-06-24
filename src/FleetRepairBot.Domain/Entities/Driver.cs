using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Required: Telegram identifier for the driver
        public long TelegramId { get; set; }
    }
}
