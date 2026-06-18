using System.Collections.Generic;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public interface IRepairRequestRepository
    {
        Task<RepairRequest> GetByIdAsync(int id);
        Task<IEnumerable<RepairRequest>> GetAllAsync();
        Task AddAsync(RepairRequest request);
        Task UpdateAsync(RepairRequest request);
    }
}
