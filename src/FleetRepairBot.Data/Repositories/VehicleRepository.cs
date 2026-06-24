using FleetRepairBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FleetRepairBot.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly FleetRepairDbContext _db;
        public VehicleRepository(FleetRepairDbContext db) => _db = db;

        public async Task AddAsync(Vehicle vehicle) => await _db.Vehicles.AddAsync(vehicle);
        public async Task<Vehicle> GetByIdAsync(Guid id) => await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        public Task UpdateAsync(Vehicle vehicle)
        {
            _db.Vehicles.Update(vehicle);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    }
}
