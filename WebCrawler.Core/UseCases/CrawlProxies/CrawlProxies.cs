using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebCrawler.Core.Entities;
using WebCrawler.Core.Ports;
using WebCrawler.Core.Settings;

namespace WebCrawler.Core.UseCases.CrawlProxies
{
    public class CrawlProxies
    {
        private static readonly SemaphoreSlim _semaphore = new(3, 3);
        
        private readonly ICrawlProxiesRepository _repository;
        private readonly IExportFiles _exportFiles;
        private readonly IDirectoryCreator _directoryCreator;
        private const string Url = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc";
        private const int TotalOfProprieties = 6;
        

        public CrawlProxies(
            ICrawlProxiesRepository repository,
            IDirectoryCreator directoryCreator,
            IExportFiles exportFiles
            )
        {
            _repository = repository;
            _exportFiles = exportFiles;
            _directoryCreator = directoryCreator;
        }

        public async Task Handle()
        {
            await _semaphore.WaitAsync();

            using (var driver = new ChromeDriver(ChromeOptionsSettings.ChromeOptions()))
            {
                try
                {
                    var initialDate = DateTime.Now;

                    await driver.Navigate().GoToUrlAsync(Url);

                    Thread.Sleep(5000);

                    var proxyList = new List<ProxyInfo>();
                    var rows = driver.FindElements(By.XPath("//table[@class='table table-hover']/tbody/tr"));

                    foreach (var row in rows)
                    {
                        var tds = row.FindElements(By.TagName("td"));
                        if (tds.Count < TotalOfProprieties) continue;

                        var port = tds[2].Text;
                        var portValue = Convert.ToInt32(port);

                        proxyList.Add(new ProxyInfo
                        {
                            IpAddress = tds[1].Text.Trim(),
                            Port = portValue,
                            Country = tds[3].Text.Trim(),
                            Protocol = tds[6].Text.Trim()
                        });
                    }

                    var jsonFile = JsonSerializer.Serialize(proxyList);

                    await SaveInfoToRepository(initialDate, proxyList.Count, jsonFile, driver.PageSource);

                    await SaveJsonFile(jsonFile);

                    await CapturePageHtml(driver.PageSource);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        private async Task SaveInfoToRepository(DateTime initialDate, int proxyList, string jsonFile, string driver)
        {
            var finalDate = DateTime.Now;
            var pageNumbers = 1;

            var dbDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "database");
            var dbPath = Path.Combine(dbDirectory, "executionsLog.db");
            
            await _directoryCreator.Create(dbDirectory, dbPath);
            await _repository.Save(initialDate, finalDate, pageNumbers, proxyList, jsonFile, driver, dbPath);
            
            Console.WriteLine("Dados da execução salvos no banco de dados.");
        }

        private async Task SaveJsonFile(string jsonFile)
        {
            var jsonDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "jsonFiles");
            var jsonFileName = Path.Combine(jsonDirectory, $"proxy_data_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");

            await _directoryCreator.Create(jsonDirectory, jsonFileName);
            await _exportFiles.WriteFile(jsonFileName, jsonFile);
        }

        private async Task CapturePageHtml(string driver)
        {
            var htmlDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "HtmlFiles");
            var htmlFileName = Path.Combine(htmlDirectory, $"pagina_web_{DateTime.UtcNow:yyyyMMdd_HHmmss}.html");

            await _directoryCreator.Create(htmlDirectory, htmlFileName);
            await _exportFiles.WriteFile(htmlFileName, driver);
        }
    }
}