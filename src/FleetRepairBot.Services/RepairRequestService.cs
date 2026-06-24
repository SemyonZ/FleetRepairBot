using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Domain.Entities;
using FleetRepairBot.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FleetRepairBot.Services
{
    public class RepairRequestService : IRepairRequestService
    {
        private readonly IRepairRequestRepository _repo;
        private readonly IFileStorage _storage;

        public RepairRequestService(IRepairRequestRepository repo, IFileStorage storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<RepairRequest> CreateRequestAsync(Guid vehicleId, Guid? driverId, string description)
        {
            var req = new RepairRequest(vehicleId, driverId, description);
            await _repo.AddAsync(req);
            await _repo.SaveChangesAsync();
            return req;
        }

        public async Task<RepairRequest> GetRequestAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task UpdateStatusAsync(Guid id, Status newStatus, string changedBy)
        {
            var req = await _repo.GetByIdAsync(id);
            if (req == null) throw new InvalidOperationException("Request not found");
            req.ChangeStatus(newStatus, changedBy);
            await _repo.UpdateAsync(req);
            await _repo.SaveChangesAsync();
        }

        public async Task<RequestPhoto> AddPhotoAsync(Guid requestId, Stream content, string fileName)
        {
            var req = await _repo.GetByIdAsync(requestId);
            if (req == null) throw new InvalidOperationException("Request not found");
            var path = $"requests/{requestId}/{Guid.NewGuid()}_{fileName}";
            var saved = await _storage.SaveAsync(content, path);
            var photo = new RequestPhoto(requestId, saved);
            req.AddPhoto(photo);
            await _repo.UpdateAsync(req);
            await _repo.SaveChangesAsync();
            return photo;
        }
    }
}
