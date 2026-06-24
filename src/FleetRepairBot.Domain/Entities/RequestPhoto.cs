using System;

namespace FleetRepairBot.Domain.Entities
{
    public class RequestPhoto
    {
        public Guid Id { get; private set; }
        public Guid RepairRequestId { get; private set; }
        public string Path { get; private set; }
        public string ThumbnailPath { get; private set; }
        public DateTime UploadedAt { get; private set; }

        protected RequestPhoto() { }

        public RequestPhoto(Guid repairRequestId, string path, string thumbnailPath = null)
        {
            Id = Guid.NewGuid();
            RepairRequestId = repairRequestId;
            Path = path ?? throw new ArgumentNullException(nameof(path));
            ThumbnailPath = thumbnailPath;
            UploadedAt = DateTime.UtcNow;
        }
    }
}
