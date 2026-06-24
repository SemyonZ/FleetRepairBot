using FleetRepairBot.Domain.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FleetRepairBot.Services
{
    public interface IRepairRequestService
    {
        Task<RepairRequest> CreateRequestAsync(Guid vehicleId, Guid? driverId, string description);
        Task<RepairRequest> GetRequestAsync(Guid id);
        Task UpdateStatusAsync(Guid id, Status newStatus, string changedBy);
        Task<RequestPhoto> AddPhotoAsync(Guid requestId, Stream content, string fileName);
    }
}
