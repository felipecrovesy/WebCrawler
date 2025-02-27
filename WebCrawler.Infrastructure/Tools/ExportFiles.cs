using WebCrawler.Core.Ports;

namespace WebCrawler.Infrastructure.Tools;

public class ExportFiles : IExportFiles
{
    public Task WriteFile(string path, string content)
    {
        File.WriteAllTextAsync(path, content);
        
        Console.WriteLine($"Dados extraídos e salvos em {path}");
        return Task.CompletedTask;
    }
}