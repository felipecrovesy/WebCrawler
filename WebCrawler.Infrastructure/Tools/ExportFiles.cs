using System.IO.Abstractions;
using WebCrawler.Core.Ports;

namespace WebCrawler.Infrastructure.Tools
{
    public class ExportFiles : IExportFiles
    {
        private readonly IFileSystem _fileSystem;

        public ExportFiles(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task WriteFile(string filePath, string content)
        {
            await using (var writer = _fileSystem.File.CreateText(filePath))
            {
                await writer.WriteAsync(content);
                Console.WriteLine($"Dados extraídos e salvos em {filePath}");
            }
        }
    }
}