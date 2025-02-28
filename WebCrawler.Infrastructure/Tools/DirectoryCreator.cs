using System.IO.Abstractions;
using WebCrawler.Core.Ports;

namespace WebCrawler.Infrastructure.Tools
{
    public class DirectoryCreator : IDirectoryCreator
    {
        private readonly IFileSystem _fileSystem;

        public DirectoryCreator(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task Create(string directoryPath, string filePath)
        {
            if (!_fileSystem.Directory.Exists(directoryPath))
            {
                _fileSystem.Directory.CreateDirectory(directoryPath);
            }

            if (!_fileSystem.File.Exists(filePath))
            {
                _fileSystem.File.Create(filePath).Dispose();
            }

            return Task.CompletedTask;
        }
    }
}