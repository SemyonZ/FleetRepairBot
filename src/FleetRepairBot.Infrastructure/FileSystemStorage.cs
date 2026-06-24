using System;
using System.IO;
using System.Threading.Tasks;

namespace FleetRepairBot.Infrastructure
{
    public class FileSystemStorage : IFileStorage
    {
        private readonly string _root;

        public FileSystemStorage(string rootDirectory)
        {
            _root = string.IsNullOrWhiteSpace(rootDirectory) ? "./files" : rootDirectory;
            Directory.CreateDirectory(_root);
        }

        public async Task DeleteAsync(string relativePath)
        {
            var full = Path.Combine(_root, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(full)) File.Delete(full);
            await Task.CompletedTask;
        }

        public async Task<Stream> OpenReadAsync(string relativePath)
        {
            var full = Path.Combine(_root, relativePath.Replace('/', Path.DirectorySeparatorChar));
            return await Task.FromResult<Stream>(File.OpenRead(full));
        }

        public string GetUri(string relativePath)
        {
            // return a server-relative path for simplicity
            return "/files/" + relativePath.Replace('\\', '/');
        }

        public async Task<string> SaveAsync(Stream content, string relativePath)
        {
            var full = Path.Combine(_root, relativePath.Replace('/', Path.DirectorySeparatorChar));
            var dir = Path.GetDirectoryName(full);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            using (var fs = File.Create(full))
            {
                await content.CopyToAsync(fs);
            }
            return relativePath.Replace('\\', '/');
        }
    }
}
