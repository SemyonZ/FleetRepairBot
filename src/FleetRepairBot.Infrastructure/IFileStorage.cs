using System.IO;
using System.Threading.Tasks;

namespace FleetRepairBot.Infrastructure
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(Stream content, string relativePath);
        Task DeleteAsync(string relativePath);
        Task<Stream> OpenReadAsync(string relativePath);
        string GetUri(string relativePath);
    }
}
