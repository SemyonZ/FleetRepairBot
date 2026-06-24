using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Application.Dto;

namespace FleetRepairBot.Application.Interfaces
{
    public interface IRepairRequestService
    {
        Task<RepairRequestDto> CreateAsync(RepairRequestCreateDto dto, CancellationToken ct = default);
        Task<RepairRequestDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<RepairRequestDto>> GetByDriverAsync(int driverTelegramId, CancellationToken ct = default);
        Task ChangeStatusAsync(int requestId, int statusId, int? dispatcherId, long performedByTelegramId, string details = null, CancellationToken ct = default);
    }
}
