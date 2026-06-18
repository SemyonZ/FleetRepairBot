using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly FleetRepairDbContext _db;
        public DriverRepository(FleetRepairDbContext db) { _db = db; }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _db.Drivers.ToListAsync();
        }

        public async Task<Driver> GetByIdAsync(int id)
        {
            return await _db.Drivers.FindAsync(id);
        }
    }
}
