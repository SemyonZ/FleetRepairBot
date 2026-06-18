using System;
using System.Collections.Generic;

namespace FleetRepairBot.Domain.Entities
{
    public class RepairRequest
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RequestPhoto> Photos { get; set; } = new List<RequestPhoto>();
    }
}
