using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebCrawler.Core.Entities;
using WebCrawler.Core.Ports;
using WebCrawler.Core.Settings;

namespace WebCrawler.Core.UseCases.CrawlProxies
{
    public class CrawlProxiesUseCase
    {
        private readonly ICrawlProxiesRepository _repository;
        private const string Url = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc";
        private const int TotalOfProprieties = 6;

        public CrawlProxiesUseCase(ICrawlProxiesRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle()
        {
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
            }
        }

        private async Task SaveInfoToRepository(DateTime initialDate, int proxyList, string jsonFile, string driver)
        {
            var finalDate = DateTime.Now;
            var pageNumbers = 1; //TODO: Consertar a lógica de Paginação

            await _repository.Save(initialDate, finalDate, pageNumbers, proxyList, jsonFile, driver);
            
            Console.WriteLine("Dados da execução salvos no banco de dados.");
        }

        private async Task SaveJsonFile(string jsonFile)
        {
            var jsonDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "jsonFiles");
            var jsonFileName = Path.Combine(jsonDirectory, $"proxy_data_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");

            await File.WriteAllTextAsync(jsonFileName, jsonFile);
            
            Console.WriteLine($"Dados extraídos e salvos em {jsonFileName}");
        }

        private async Task CapturePageHtml(string driver)
        {
            var htmlDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "HtmlFiles");
            var htmlFileName = Path.Combine(htmlDirectory, $"pagina_web_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            
            await File.WriteAllTextAsync(htmlFileName, driver);
            
            Console.WriteLine($"Página salva em {htmlFileName}");
        }
    }
}