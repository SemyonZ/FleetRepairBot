using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public class RepairRequestRepository : IRepairRequestRepository
    {
        private readonly FleetRepairDbContext _db;
        public RepairRequestRepository(FleetRepairDbContext db) { _db = db; }

        public async Task AddAsync(RepairRequest request)
        {
            _db.RepairRequests.Add(request);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<RepairRequest>> GetAllAsync()
        {
            return await _db.RepairRequests.Include(r => r.Photos).ToListAsync();
        }

        public async Task<RepairRequest> GetByIdAsync(int id)
        {
            return await _db.RepairRequests.Include(r => r.Photos).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(RepairRequest request)
        {
            _db.RepairRequests.Update(request);
            await _db.SaveChangesAsync();
        }
    }
}
