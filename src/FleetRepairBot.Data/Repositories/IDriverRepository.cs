using System.Collections.Generic;
using System.Threading.Tasks;
using FleetRepairBot.Domain.Entities;

namespace FleetRepairBot.Data.Repositories
{
    public interface IDriverRepository
    {
        Task<Driver> GetByIdAsync(int id);
        Task<IEnumerable<Driver>> GetAllAsync();
    }
}
