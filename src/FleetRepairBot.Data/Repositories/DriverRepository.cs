using FleetRepairBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FleetRepairBot.Data.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly FleetRepairDbContext _db;
        public DriverRepository(FleetRepairDbContext db) => _db = db;

        public async Task AddAsync(Driver driver) => await _db.Drivers.AddAsync(driver);
        public async Task<Driver> GetByIdAsync(Guid id) => await _db.Drivers.FirstOrDefaultAsync(d => d.Id == id);
        public Task UpdateAsync(Driver driver)
        {
            _db.Drivers.Update(driver);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    }
}
