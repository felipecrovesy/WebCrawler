namespace WebCrawler.Core.Ports;

public interface IExportFiles
{
    Task WriteFile(string path, string content);
}