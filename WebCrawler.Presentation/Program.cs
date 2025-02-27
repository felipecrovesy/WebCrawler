using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebCrawler.Application.Services;
using WebCrawler.Core.Entities;
using WebCrawler.Core.Interfaces;
using WebCrawler.Infrastructure.Data;
using WebCrawler.Infrastructure.Repositories;
using WebCrawler.Infrastructure.Services;
using static WebCrawler.Infrastructure.Services.WebCrawlerInfrastructureService;

namespace WebCrawler.Presentation;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        var webCrawlerService = serviceProvider.GetService<IWebCrawlerService>();
        var proxyRepository = serviceProvider.GetService<IProxyRepository>();
        serviceProvider.GetService<IConfiguration>();
        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();

        string baseUrl = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc";
        int totalPages = await webCrawlerService.GetTotalPages(baseUrl);

        int maxThreads = 3;
        SemaphoreSlim semaphore = new SemaphoreSlim(maxThreads, maxThreads);
        List<Task> tasks = new List<Task>();
        List<ProxyInfo> allProxies = new List<ProxyInfo>();
        var startTime = DateTime.Now;

        for (int page = 1; page <= totalPages; page++)
        {
            string url = $"{baseUrl}/page/{page}";
            tasks.Add(Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var proxies = await webCrawlerService.Crawl(url);
                    lock(allProxies) {allProxies.AddRange(proxies);}
                    await SaveHtml(url, clientFactory);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
        var endTime = DateTime.Now;

        string jsonFilePath = "proxies.json";
        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(allProxies));

        using (var context = serviceProvider.GetService<IDbContextFactory<WebCrawlerContext>>().CreateDbContext())
        {
            proxyRepository.SaveExecutionLog(new ExecutionLog
            {
                StartTime = startTime,
                EndTime = endTime,
                PageCount = totalPages,
                LineCount = allProxies.Count,
                JsonFilePath = jsonFilePath
            });
        }
        Console.WriteLine($"Extração concluída. Total de proxies encontrados: {allProxies.Count}");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddHttpClient();
        services.AddDbContextFactory<WebCrawlerContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IWebCrawlerService, WebCrawlerService>();
        services.AddScoped<ICrawlerInfrastructureService, CrawlerInfrastructureService>();
        services.AddScoped<IProxyRepository, ProxyRepository>();
    }

    static async Task SaveHtml(string url, IHttpClientFactory clientFactory)
    {
        using (var client = clientFactory.CreateClient())
        {
            var html = await client.GetStringAsync(url);
            var pageNumber = url.Substring(url.LastIndexOf('/') + 1);
            if(pageNumber.Contains('?')) {
                pageNumber = pageNumber.Substring(0, pageNumber.IndexOf('?'));
            }

            if(pageNumber == "desc") {
                pageNumber = "1";
            }

            File.WriteAllText($"page_{pageNumber}.html", html);
        }
    }
}