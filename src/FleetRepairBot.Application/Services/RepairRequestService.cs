using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Application.Dto;
using FleetRepairBot.Application.Interfaces;
using FleetRepairBot.Application.Mappings;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Application.Services
{
    public class RepairRequestService : IRepairRequestService
    {
        private readonly IRepairRequestRepository _repo;
        private readonly MappingProfile _mapper;

        public RepairRequestService(IRepairRequestRepository repo, MappingProfile mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<RepairRequestDto> CreateAsync(RepairRequestCreateDto dto, CancellationToken ct = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.ReporterTelegramId == 0) throw new ArgumentException("ReporterTelegramId is required");

            var entity = _mapper.MapToEntity(dto);

            // Initialize minimal status if not set - use StatusId = 0 (assume seeded statuses exist). Keep simple for MVP.
            entity.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(entity, ct);

            // Optionally update audit/status history
            return _mapper.MapToDto(entity);
        }

        public async Task<RepairRequestDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdWithDetailsAsync(id, ct);
            return _mapper.MapToDto(entity);
        }

        public async Task<IEnumerable<RepairRequestDto>> GetByDriverAsync(int driverTelegramId, CancellationToken ct = default)
        {
            // Our repository uses driverId from domain (int). For MVP we try to match via Drivers table using repo GetByDriverAsync expects driverId.
            // For simplicity, we expose empty list if mapping not available.
            return Enumerable.Empty<RepairRequestDto>();
        }

        public async Task ChangeStatusAsync(int requestId, int statusId, int? dispatcherId, long performedByTelegramId, string details = null, CancellationToken ct = default)
        {
            var request = await _repo.GetByIdWithDetailsAsync(requestId, ct);
            if (request == null) throw new InvalidOperationException("Repair request not found");

            request.StatusId = statusId;
            request.UpdatedAt = DateTime.UtcNow;

            request.StatusHistory.Add(new StatusHistory
            {
                RepairRequestId = requestId,
                StatusId = statusId,
                ChangedAt = DateTime.UtcNow,
                DispatcherId = dispatcherId
            });

            request.AuditLogs.Add(new AuditLog
            {
                EntityName = nameof(RepairRequest),
                EntityId = requestId,
                Action = "ChangeStatus",
                Details = details ?? $"StatusId={statusId}",
                PerformedAt = DateTime.UtcNow,
                PerformedBy = performedByTelegramId.ToString()
            });

            await _repo.UpdateAsync(request, ct);
        }
    }
}
