using System;

namespace FleetRepairBot.Domain.Entities
{
    public class RequestPhoto
    {
        public Guid Id { get; set; }
        public Guid RepairRequestId { get; set; }
        public RepairRequest RepairRequest { get; set; }

        // stored path or blob key
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long? SizeBytes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
