using System.Data.SQLite;
using WebCrawler.Core.Ports;

namespace WebCrawler.Infrastructure.Repositories;

public class CrawlProxiesRepository : ICrawlProxiesRepository
{
    private static readonly string DbDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "database");
    private static readonly string DbPath = Path.Combine(DbDirectory, "executionsLog.db");

    public Task Save(
        DateTime initialDate, 
        DateTime finalDate, 
        int pageNumbers, 
        int numberLines,
        string jsonFile, 
        string pageSource)
    {
        if (!Directory.Exists(DbDirectory))
        {
            Directory.CreateDirectory(DbDirectory);
        }

        if (File.Exists(DbPath)) return Task.CompletedTask;
        
        SQLiteConnection.CreateFile(DbPath);
        
        using (var connection = new SQLiteConnection($"Data Source={DbPath}"))
        {
            connection.Open();
            var createTableQuery = @"
                    CREATE TABLE ExecutionsLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        DataInicio TEXT,
                        DataTermino TEXT,
                        QuantidadePaginas INTEGER,
                        QuantidadeLinhas INTEGER,
                        ArquivoJson TEXT
                    );
                ";
            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        return Task.CompletedTask;
    }
}