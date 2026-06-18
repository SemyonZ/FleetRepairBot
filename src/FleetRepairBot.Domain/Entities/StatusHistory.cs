using System;

namespace FleetRepairBot.Domain.Entities
{
    public class StatusHistory
    {
        public int Id { get; set; }
        public int RepairRequestId { get; set; }
        public Status OldStatus { get; set; }
        public Status NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
