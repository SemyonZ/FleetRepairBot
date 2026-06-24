using System;
using FleetRepairBot.Application.Dto;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Application.Mappings
{
    // Minimal manual mapping helpers for MVP
    public class MappingProfile
    {
        public RepairRequest MapToEntity(RepairRequestCreateDto dto)
        {
            var r = new RepairRequest
            {
                Title = string.IsNullOrWhiteSpace(dto.Title) ? (dto.Description?.Length > 50 ? dto.Description.Substring(0, 50) : dto.Description) : dto.Title,
                Description = dto.Description,
                VehicleId = dto.VehicleId ?? 0,
                CreatedAt = DateTime.UtcNow
            };
            return r;
        }

        public RepairRequestDto MapToDto(RepairRequest entity)
        {
            if (entity == null) return null;
            return new RepairRequestDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                VehicleId = entity.VehicleId,
                ReporterTelegramId = entity.Driver != null ? entity.Driver.TelegramId : 0,
                Status = entity.Status?.Name,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
