using System;
using System.Collections.Generic;
using System.Linq;

namespace FleetRepairBot.Domain.Entities
{
    public class RepairRequest
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid? DriverId { get; private set; }
        public Guid? DispatcherId { get; private set; }
        public Status Status { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private readonly List<RequestPhoto> _photos = new List<RequestPhoto>();
        public IReadOnlyCollection<RequestPhoto> Photos => _photos.AsReadOnly();

        private readonly List<StatusHistory> _statusHistory = new List<StatusHistory>();
        public IReadOnlyCollection<StatusHistory> StatusHistory => _statusHistory.AsReadOnly();

        protected RepairRequest() { }

        public RepairRequest(Guid vehicleId, Guid? driverId, string description)
        {
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            DriverId = driverId;
            Description = description ?? string.Empty;
            Status = Status.New;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
            _statusHistory.Add(new StatusHistory(Id, null, Status.New, "system", CreatedAt));
        }

        public void AssignDispatcher(Guid dispatcherId)
        {
            DispatcherId = dispatcherId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeStatus(Status newStatus, string changedBy)
        {
            if (newStatus == Status)
                return;

            // basic rule: cannot move from Completed/Cancelled to other states
            if ((Status == Status.Completed || Status == Status.Cancelled) && newStatus != Status)
                throw new InvalidOperationException("Cannot change status from terminal state.");

            var old = Status;
            Status = newStatus;
            var now = DateTime.UtcNow;
            _statusHistory.Add(new StatusHistory(Id, old, newStatus, changedBy ?? "unknown", now));
            UpdatedAt = now;
        }

        public void AddPhoto(RequestPhoto photo)
        {
            if (photo == null) throw new ArgumentNullException(nameof(photo));
            _photos.Add(photo);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
