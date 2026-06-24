using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Domain.Entities;
using FleetRepairBot.Infrastructure;

namespace FleetRepairBot.Services
{
    public class RepairRequestService : IRepairRequestService
    {
        private readonly IRepairRequestRepository _requestRepo;
        private readonly IFileStorage _fileStorage;

        public RepairRequestService(IRepairRequestRepository requestRepo, IFileStorage fileStorage)
        {
            _requestRepo = requestRepo;
            _fileStorage = fileStorage;
        }

        public async Task<RepairRequest> CreateAsync(RepairRequest request, CancellationToken ct = default)
        {
            request.CreatedAt = DateTime.UtcNow;
            await _requestRepo.AddAsync(request, ct);
            return request;
        }

        public async Task<RepairRequest> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _requestRepo.GetByIdWithDetailsAsync(id, ct);
        }

        public async Task<IEnumerable<RepairRequest>> GetByDriverAsync(int driverId, CancellationToken ct = default)
        {
            return await _requestRepo.GetByDriverAsync(driverId, ct);
        }

        public async Task AddPhotoAsync(int requestId, Stream photoStream, string fileName, long performedByTelegramId, CancellationToken ct = default)
        {
            var request = await _requestRepo.GetByIdWithDetailsAsync(requestId, ct);
            if (request == null) throw new InvalidOperationException("Repair request not found");

            var savedPath = await _fileStorage.SaveFileAsync(photoStream, fileName, ct);

            var photo = new RequestPhoto
            {
                RepairRequestId = requestId,
                FilePath = savedPath,
                UploadedAt = DateTime.UtcNow
            };

            request.Photos.Add(photo);

            request.UpdatedAt = DateTime.UtcNow;
            request.AuditLogs.Add(new AuditLog
            {
                EntityName = nameof(RepairRequest),
                EntityId = requestId,
                Action = "AddPhoto",
                Details = savedPath,
                PerformedAt = DateTime.UtcNow,
                PerformedBy = performedByTelegramId.ToString()
            });

            await _requestRepo.UpdateAsync(request, ct);
        }

        public async Task ChangeStatusAsync(int requestId, int statusId, int? dispatcherId, long performedByTelegramId, string details = null, CancellationToken ct = default)
        {
            var request = await _requestRepo.GetByIdWithDetailsAsync(requestId, ct);
            if (request == null) throw new InvalidOperationException("Repair request not found");

            request.StatusId = statusId;
            request.UpdatedAt = DateTime.UtcNow;

            var history = new StatusHistory
            {
                RepairRequestId = requestId,
                StatusId = statusId,
                ChangedAt = DateTime.UtcNow,
                DispatcherId = dispatcherId
            };

            request.StatusHistory.Add(history);

            request.AuditLogs.Add(new AuditLog
            {
                EntityName = nameof(RepairRequest),
                EntityId = requestId,
                Action = "ChangeStatus",
                Details = details ?? $"StatusId={statusId}",
                PerformedAt = DateTime.UtcNow,
                PerformedBy = performedByTelegramId.ToString()
            });

            await _requestRepo.UpdateAsync(request, ct);
        }
    }
}
