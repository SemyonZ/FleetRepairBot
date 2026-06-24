using FleetRepairBot.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FleetRepairBot.Data.Repositories
{
    public interface IDriverRepository
    {
        Task<Driver> GetByIdAsync(Guid id);
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);
        Task SaveChangesAsync();
    }
}
