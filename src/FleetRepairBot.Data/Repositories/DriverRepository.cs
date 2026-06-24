using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly FleetRepairDbContext _context;

        public DriverRepository(FleetRepairDbContext context)
        {
            _context = context;
        }

        public async Task<Driver> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Drivers.FindAsync(new object[] { id }, ct);
        }

        public async Task<Driver> GetByTelegramIdAsync(long telegramId, CancellationToken ct = default)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.TelegramId == telegramId, ct);
        }

        public async Task<IEnumerable<Driver>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Drivers.OrderBy(d => d.Name).ToListAsync(ct);
        }

        public async Task AddAsync(Driver driver, CancellationToken ct = default)
        {
            await _context.Drivers.AddAsync(driver, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Driver driver, CancellationToken ct = default)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync(ct);
        }
    }
}
