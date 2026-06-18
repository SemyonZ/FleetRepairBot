using System;
using System.IO;
using System.Threading.Tasks;

namespace FleetRepairBot.Infrastructure
{
    public class FileSystemStorage : IFileStorage
    {
        private readonly string _basePath;
        public FileSystemStorage(string basePath)
        {
            _basePath = string.IsNullOrWhiteSpace(basePath) ? "./files" : basePath;
            Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveAsync(Stream data, string fileName)
        {
            var path = Path.Combine(_basePath, fileName);
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await data.CopyToAsync(fs);
            }
            return path;
        }
    }
}
