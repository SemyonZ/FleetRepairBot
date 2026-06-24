using System;
using System.Collections.Generic;

namespace FleetRepairBot.Domain.Entities
{
    // Represents a repair request (not an order)
    public class RepairRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; }

        // Assigned dispatcher (optional)
        public int? DispatcherId { get; set; }
        public Dispatcher Dispatcher { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<RequestPhoto> Photos { get; set; } = new List<RequestPhoto>();
        public ICollection<StatusHistory> StatusHistory { get; set; } = new List<StatusHistory>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
