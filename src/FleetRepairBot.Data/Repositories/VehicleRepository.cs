using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly FleetRepairDbContext _db;
        public VehicleRepository(FleetRepairDbContext db) { _db = db; }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _db.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _db.Vehicles.FindAsync(id);
        }
    }
}
