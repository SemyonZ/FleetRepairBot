using System.IO;
using System.Threading.Tasks;

namespace FleetRepairBot.Infrastructure
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(Stream data, string fileName);
    }
}
