namespace WebCrawler.Core.Ports;

public interface IDirectoryCreator
{
    Task Create(string directory, string path);
}