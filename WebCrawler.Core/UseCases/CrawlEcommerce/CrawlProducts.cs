using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebCrawler.Core.Entities;
using WebCrawler.Core.Ports;
using WebCrawler.Core.Settings;
using HtmlAgilityPack;

namespace WebCrawler.Core.UseCases.CrawlEcommerce
{
    public class CrawlProducts
    {
        private static readonly SemaphoreSlim _semaphore = new(3, 3);

        private readonly ICrawlProductsRepository _repository;
        private readonly IExportFiles _exportFiles;
        private readonly IDirectoryCreator _directoryCreator;

        public CrawlProducts(
            ICrawlProductsRepository repository,
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
                    var currentPage = 1;
                    var hasNextPage = true;

                    while (hasNextPage)
                    {
                        var url = $"https://www.scrapingcourse.com/ecommerce/page/{currentPage}/";

                        await driver.Navigate().GoToUrlAsync(url);

                        Thread.Sleep(5000);

                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(driver.PageSource);

                        var productList = new List<ProductInfo>();
                        var productHTMLElements = htmlDoc.DocumentNode.SelectNodes("//li[contains(@class, 'product')]");

                        if (productHTMLElements != null)
                        {
                            foreach (var productHTMLElement in productHTMLElements)
                            {
                                try
                                {
                                    var nameNode = productHTMLElement.SelectSingleNode(".//h2[contains(@class, 'product-name')]");
                                    var priceNode = productHTMLElement.SelectSingleNode(".//span[contains(@class, 'product-price')]");
                                    var imageNode = productHTMLElement.SelectSingleNode(".//img[contains(@class, 'attachment-woocommerce_thumbnail')]"); // Novo seletor XPath
                                    
                                    if (nameNode != null && priceNode != null && imageNode != null)
                                    {
                                        var name = HtmlEntity.DeEntitize(nameNode.InnerText);
                                        var price = HtmlEntity.DeEntitize(priceNode.InnerText);
                                        var image = HtmlEntity.DeEntitize(imageNode.Attributes["src"].Value);

                                        productList.Add(new ProductInfo
                                        {
                                            Name = name,
                                            Price = price,
                                            Image = image
                                        });
                                    }
                                    else
                                    {
                                        Console.WriteLine("Um ou mais nós não foram encontrados para este produto.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Erro ao extrair dados do produto: {ex.Message}");
                                }
                            }
                        }

                        var jsonFile = JsonSerializer.Serialize(productList);

                        await SaveInfoToRepository(initialDate, productList.Count, jsonFile, driver.PageSource, currentPage);

                        await SaveJsonFile(jsonFile, currentPage);

                        await CapturePageHtml(driver.PageSource, currentPage);

                        try
                        {
                            var nextPageElement = driver.FindElement(By.XPath("//a[@class='next page-numbers']"));
                            hasNextPage = nextPageElement != null;
                        }
                        catch (NoSuchElementException)
                        {
                            hasNextPage = false;
                        }

                        currentPage++;
                    }
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

        private async Task SaveInfoToRepository(DateTime initialDate, int productListCount, string jsonFile, string driver, int pageNumbers)
        {
            var finalDate = DateTime.Now;

            var dbDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "database");
            var dbPath = Path.Combine(dbDirectory, "executionsCrawProductsLog.db");

            await _directoryCreator.Create(dbDirectory, dbPath);
            await _repository.Save(initialDate, finalDate, pageNumbers, productListCount, jsonFile, driver, dbPath);
        }

        private async Task SaveJsonFile(string jsonFile, int currentPage)
        {
            var jsonDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "jsonFiles");
            var jsonFileName = Path.Combine(jsonDirectory, $"product_data_page_{currentPage}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");

            await _directoryCreator.Create(jsonDirectory, jsonFileName);
            await _exportFiles.WriteFile(jsonFileName, jsonFile);
        }

        private async Task CapturePageHtml(string driver, int currentPage)
        {
            var htmlDirectory = Path.Combine(AppContext.BaseDirectory, "assets", "HtmlFiles");
            var htmlFileName = Path.Combine(htmlDirectory, $"ecommerce_web_page_{currentPage}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.html");

            await _directoryCreator.Create(htmlDirectory, htmlFileName);
            await _exportFiles.WriteFile(htmlFileName, driver);
        }
    }
}