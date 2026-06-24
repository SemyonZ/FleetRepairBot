using System;

namespace FleetRepairBot.Domain.Entities
{
    public class RequestPhoto
    {
        public int Id { get; set; }
        public int RepairRequestId { get; set; }
        public RepairRequest RepairRequest { get; set; }

        // Stored path or URL to the uploaded photo
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
