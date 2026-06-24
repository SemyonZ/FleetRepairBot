using System;

namespace FleetRepairBot.Domain.Entities
{
    public class StatusHistory
    {
        public Guid Id { get; private set; }
        public Guid RepairRequestId { get; private set; }
        public Status? From { get; private set; }
        public Status To { get; private set; }
        public string ChangedBy { get; private set; }
        public DateTime ChangedAt { get; private set; }

        protected StatusHistory() { }

        public StatusHistory(Guid repairRequestId, Status? from, Status to, string changedBy, DateTime changedAt)
        {
            Id = Guid.NewGuid();
            RepairRequestId = repairRequestId;
            From = from;
            To = to;
            ChangedBy = changedBy ?? "system";
            ChangedAt = changedAt;
        }
    }
}
