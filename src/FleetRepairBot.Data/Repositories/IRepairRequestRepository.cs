using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public interface IRepairRequestRepository
    {
        Task<RepairRequest> GetByIdAsync(int id, CancellationToken ct = default);
        Task<RepairRequest> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<RepairRequest>> GetByDriverAsync(int driverId, CancellationToken ct = default);
        Task AddAsync(RepairRequest request, CancellationToken ct = default);
        Task UpdateAsync(RepairRequest request, CancellationToken ct = default);
    }
}
