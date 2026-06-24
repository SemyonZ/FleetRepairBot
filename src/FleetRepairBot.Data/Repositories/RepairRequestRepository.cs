using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public class RepairRequestRepository : IRepairRequestRepository
    {
        private readonly FleetRepairDbContext _context;

        public RepairRequestRepository(FleetRepairDbContext context)
        {
            _context = context;
        }

        public async Task<RepairRequest> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.RepairRequests.FindAsync(new object[] { id }, ct);
        }

        public async Task<RepairRequest> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await _context.RepairRequests
                .Include(r => r.Vehicle)
                .Include(r => r.Driver)
                .Include(r => r.Dispatcher)
                .Include(r => r.Status)
                .Include(r => r.Photos)
                .Include(r => r.StatusHistory)
                .Include(r => r.AuditLogs)
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<IEnumerable<RepairRequest>> GetByDriverAsync(int driverId, CancellationToken ct = default)
        {
            return await _context.RepairRequests
                .Where(r => r.DriverId == driverId)
                .Include(r => r.Status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task AddAsync(RepairRequest request, CancellationToken ct = default)
        {
            await _context.RepairRequests.AddAsync(request, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(RepairRequest request, CancellationToken ct = default)
        {
            _context.RepairRequests.Update(request);
            await _context.SaveChangesAsync(ct);
        }
    }
}
