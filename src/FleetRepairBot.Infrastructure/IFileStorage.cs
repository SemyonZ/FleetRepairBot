using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FleetRepairBot.Infrastructure
{
    public interface IFileStorage
    {
        /// <summary>
        /// Saves provided stream to storage and returns a path or URL that can be stored in DB.
        /// </summary>
        Task<string> SaveFileAsync(Stream stream, string fileName, CancellationToken ct = default);

        /// <summary>
        /// Deletes file by path if exists.
        /// </summary>
        Task DeleteFileAsync(string path, CancellationToken ct = default);
    }
}
