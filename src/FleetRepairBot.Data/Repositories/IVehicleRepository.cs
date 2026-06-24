using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Vehicle> GetByVinAsync(string vin, CancellationToken ct = default);
        Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Vehicle vehicle, CancellationToken ct = default);
        Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default);
    }
}
