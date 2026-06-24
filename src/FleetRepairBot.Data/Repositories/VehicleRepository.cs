using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly FleetRepairDbContext _context;

        public VehicleRepository(FleetRepairDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Vehicles.FindAsync(new object[] { id }, ct);
        }

        public async Task<Vehicle> GetByVinAsync(string vin, CancellationToken ct = default)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.VIN == vin, ct);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Vehicles.OrderBy(v => v.LicensePlate).ToListAsync(ct);
        }

        public async Task AddAsync(Vehicle vehicle, CancellationToken ct = default)
        {
            await _context.Vehicles.AddAsync(vehicle, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync(ct);
        }
    }
}
