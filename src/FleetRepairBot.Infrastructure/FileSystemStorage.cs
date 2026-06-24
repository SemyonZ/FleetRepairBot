using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FleetRepairBot.Infrastructure;

namespace FleetRepairBot.Infrastructure
{
    public class FileSystemStorage : IFileStorage
    {
        private readonly string _basePath;

        public FileSystemStorage(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath)) throw new ArgumentException(nameof(basePath));
            _basePath = basePath;
        }

        /// <summary>
        /// Saves provided stream to storage and returns a relative path that can be stored in DB.
        /// The returned path uses forward slashes and is relative to the configured base path.
        /// </summary>
        public async Task<string> SaveFileAsync(Stream stream, string fileName, CancellationToken ct = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException(nameof(fileName));

            var safeFileName = Path.GetFileName(fileName);
            var relativePath = Path.Combine("uploads", DateTime.UtcNow.ToString("yyyyMMdd"), $"{Guid.NewGuid():N}_{safeFileName}");
            var fullPath = Path.Combine(_basePath, relativePath);

            var dir = Path.GetDirectoryName(fullPath) ?? _basePath;
            Directory.CreateDirectory(dir);

            // Write stream to file
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await stream.CopyToAsync(fileStream, 81920, ct).ConfigureAwait(false);
            }

            // Return relative path with normalized separators
            return relativePath.Replace('\\', '/');
        }

        /// <summary>
        /// Deletes file by path if exists. Path is expected to be the relative path previously returned by SaveFileAsync.
        /// This is a best-effort deletion; exceptions are swallowed to avoid impacting calling business logic.
        /// </summary>
        public Task DeleteFileAsync(string path, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(path)) return Task.CompletedTask;

            var fullPath = Path.Combine(_basePath, path);
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // swallow exceptions intentionally
            }

            return Task.CompletedTask;
        }

        // Private helper retained for compatibility/testing if needed
        private async Task<string> SaveFileAsync(string relativePath, byte[] content)
        {
            var fullPath = Path.Combine(_basePath, relativePath ?? string.Empty);
            var dir = Path.GetDirectoryName(fullPath) ?? _basePath;
            Directory.CreateDirectory(dir);
            await File.WriteAllBytesAsync(fullPath, content ?? Array.Empty<byte>());
            return (relativePath ?? string.Empty).Replace('\\', '/');
        }
    }
}
