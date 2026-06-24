using System;

namespace FleetRepairBot.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public int EntityId { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        // Optionally reference who performed the action (dispatcher or driver TelegramId string)
        public string PerformedBy { get; set; }
    }
}
