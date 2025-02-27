using System.Data.SQLite;
using WebCrawler.Core.Ports;

namespace WebCrawler.Infrastructure.Repositories;

public class CrawlProxiesRepository : ICrawlProxiesRepository
{
    public Task Save(
        DateTime initialDate, 
        DateTime finalDate, 
        int pageNumbers, 
        int numberLines,
        string jsonFile, 
        string pageSource,
        string dbPath)
    {
        SQLiteConnection.CreateFile(dbPath);
        
        using (var connection = new SQLiteConnection($"Data Source={dbPath}"))
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