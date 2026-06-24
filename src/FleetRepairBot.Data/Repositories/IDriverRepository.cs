using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public interface IDriverRepository
    {
        Task<Driver> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Driver> GetByTelegramIdAsync(long telegramId, CancellationToken ct = default);
        Task<IEnumerable<Driver>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Driver driver, CancellationToken ct = default);
        Task UpdateAsync(Driver driver, CancellationToken ct = default);
    }
}
