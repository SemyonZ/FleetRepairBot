using FleetRepairBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetRepairBot.Data.Repositories
{
    public class RepairRequestRepository : IRepairRequestRepository
    {
        private readonly FleetRepairDbContext _db;
        public RepairRequestRepository(FleetRepairDbContext db) => _db = db;

        public async Task AddAsync(RepairRequest request)
        {
            await _db.RepairRequests.AddAsync(request);
        }

        public async Task<RepairRequest> GetByIdAsync(Guid id)
        {
            return await _db.RepairRequests
                .Include(r => r.Photos)
                .Include(r => r.StatusHistory)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<IEnumerable<RepairRequest>> ListByStatusAsync(Status status)
        {
            var q = _db.RepairRequests.Where(r => r.Status == status).AsQueryable();
            return Task.FromResult<IEnumerable<RepairRequest>>(q.AsEnumerable());
        }

        public Task UpdateAsync(RepairRequest request)
        {
            _db.RepairRequests.Update(request);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
