using System;

namespace FleetRepairBot.Domain.Entities
{
    public class StatusHistory
    {
        public int Id { get; set; }
        public int RepairRequestId { get; set; }
        public RepairRequest RepairRequest { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        // Who changed the status (optional dispatcher reference)
        public int? DispatcherId { get; set; }
        public Dispatcher Dispatcher { get; set; }
    }
}
