using System.Data.SQLite;
using WebCrawler.Core.Ports;

namespace WebCrawler.Infrastructure.Repositories;

public class CrawlProductsRepository : ICrawlProductsRepository
{
    public async Task Save(
        DateTime initialDate,
        DateTime finalDate,
        int pageNumbers,
        int productListCount,
        string jsonFile,
        string pageSource,
        string dbPath)
    {
        try
        {
            await Task.Run(() =>
            {
                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"
                                CREATE TABLE IF NOT EXISTS ExecutionsCrawProductsLog (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    DataInicio TEXT,
                                    DataTermino TEXT,
                                    QuantidadePaginas INTEGER,
                                    QuantidadeLinhas INTEGER,
                                    ArquivoJson TEXT
                                );";
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"
                                INSERT INTO ExecutionsCrawProductsLog (DataInicio, DataTermino, QuantidadePaginas, QuantidadeLinhas, ArquivoJson)
                                VALUES (@DataInicio, @DataTermino, @QuantidadePaginas, @QuantidadeLinhas, @ArquivoJson);";

                        command.Parameters.AddWithValue("@DataInicio", initialDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@DataTermino", finalDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@QuantidadePaginas", pageNumbers);
                        command.Parameters.AddWithValue("@QuantidadeLinhas", productListCount);
                        command.Parameters.AddWithValue("@ArquivoJson", jsonFile);

                        command.ExecuteNonQuery();
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar dados no banco de dados: {ex.Message}");
        }
    }
}