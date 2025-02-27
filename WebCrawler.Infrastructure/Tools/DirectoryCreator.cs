using WebCrawler.Core.Ports;

namespace WebCrawler.Infrastructure.Tools;

public class DirectoryCreator : IDirectoryCreator
{
    public Task Create(string directory, string path)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        return Task.FromResult(File.Exists(path) ? Task.FromResult(Task.CompletedTask) : Task.CompletedTask);
    }
}