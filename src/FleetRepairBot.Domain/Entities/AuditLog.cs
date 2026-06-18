using System;

namespace FleetRepairBot.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Entity { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
