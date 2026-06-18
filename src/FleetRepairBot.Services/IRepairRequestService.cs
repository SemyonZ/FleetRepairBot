using System.Collections.Generic;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Services
{
    public interface IRepairRequestService
    {
        Task<RepairRequest> GetAsync(int id);
        Task<IEnumerable<RepairRequest>> ListAsync();
        Task CreateAsync(RepairRequest request);
        Task UpdateAsync(RepairRequest request);
    }
}
