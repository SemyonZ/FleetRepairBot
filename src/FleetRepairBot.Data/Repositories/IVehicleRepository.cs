using System.Collections.Generic;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetByIdAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
    }
}
