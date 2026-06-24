using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Services
{
    public interface IRepairRequestService
    {
        Task<RepairRequest> CreateAsync(RepairRequest request, CancellationToken ct = default);
        Task<RepairRequest> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<RepairRequest>> GetByDriverAsync(int driverId, CancellationToken ct = default);
        Task AddPhotoAsync(int requestId, Stream photoStream, string fileName, long performedByTelegramId, CancellationToken ct = default);
        Task ChangeStatusAsync(int requestId, int statusId, int? dispatcherId, long performedByTelegramId, string details = null, CancellationToken ct = default);
    }
}
