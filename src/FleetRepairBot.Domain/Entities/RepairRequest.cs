using System;
using System.Collections.Generic;

namespace FleetRepairBot.Domain.Entities
{
    public class RepairRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Status Status { get; set; }

        public Guid? VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public Guid? DriverId { get; set; }
        public Driver Driver { get; set; }

        // who submitted the request (dispatcher)
        public Guid? CreatedByDispatcherId { get; set; }
        public Dispatcher CreatedByDispatcher { get; set; }

        public ICollection<RequestPhoto> Photos { get; set; } = new List<RequestPhoto>();
        public ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
    }
}
