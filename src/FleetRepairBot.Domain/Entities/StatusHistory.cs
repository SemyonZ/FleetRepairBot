using System;

namespace FleetRepairBot.Domain.Entities
{
    public class StatusHistory
    {
        public Guid Id { get; set; }
        public Guid RepairRequestId { get; set; }
        public RepairRequest RepairRequest { get; set; }

        public Status From { get; set; }
        public Status To { get; set; }
        public DateTime ChangedAt { get; set; }

        // optional: who changed (dispatcher TelegramId)
        public long? ChangedByTelegramId { get; set; }
        public string Note { get; set; }
    }
}
