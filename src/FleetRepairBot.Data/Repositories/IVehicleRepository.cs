using FleetRepairBot.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FleetRepairBot.Data.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetByIdAsync(Guid id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task SaveChangesAsync();
    }
}
