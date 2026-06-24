using FleetRepairBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetRepairBot.Data.Repositories
{
    public interface IRepairRequestRepository
    {
        Task<RepairRequest> GetByIdAsync(Guid id);
        Task AddAsync(RepairRequest request);
        Task UpdateAsync(RepairRequest request);
        Task<IEnumerable<RepairRequest>> ListByStatusAsync(Status status);
        Task SaveChangesAsync();
    }
}
