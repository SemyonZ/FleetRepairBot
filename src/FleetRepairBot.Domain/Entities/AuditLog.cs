using System;

namespace FleetRepairBot.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; private set; }
        public string Entity { get; private set; }
        public string Operation { get; private set; }
        public string Subject { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Details { get; private set; }

        protected AuditLog() { }

        public AuditLog(string entity, string operation, string subject, string details = null)
        {
            Id = Guid.NewGuid();
            Entity = entity ?? string.Empty;
            Operation = operation ?? string.Empty;
            Subject = subject ?? string.Empty;
            Details = details ?? string.Empty;
            Timestamp = DateTime.UtcNow;
        }
    }
}
