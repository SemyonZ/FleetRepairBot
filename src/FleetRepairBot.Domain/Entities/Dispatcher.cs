using System;

namespace FleetRepairBot.Domain.Entities
{
    public class Dispatcher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Required: Telegram identifier for the dispatcher
        public long TelegramId { get; set; }
        // Note: intentionally no Email property as per requirements
    }
}
