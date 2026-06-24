using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FleetRepairBot.Application.Interfaces
{
    public interface ITelegramClient
    {
        Task SendTextAsync(long chatId, string text, CancellationToken ct = default);
        Task SendPhotoAsync(long chatId, Stream photoStream, string fileName, string caption = null, CancellationToken ct = default);
        Task<Stream> DownloadFileAsync(string fileId, CancellationToken ct = default);
    }
}
