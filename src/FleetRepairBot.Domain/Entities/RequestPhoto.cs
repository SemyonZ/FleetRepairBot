using System;

namespace FleetRepairBot.Domain.Entities
{
    public class RequestPhoto
    {
        public int Id { get; set; }
        public int RepairRequestId { get; set; }
        public string FilePath { get; set; }
    }
}
