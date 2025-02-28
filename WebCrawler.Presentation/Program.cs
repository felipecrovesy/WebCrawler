using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCrawler.Core.Ports;
using WebCrawler.Core.UseCases.CrawlEcommerce;
using WebCrawler.Core.UseCases.CrawlProxies;
using WebCrawler.Infrastructure.Repositories;
using WebCrawler.Infrastructure.Tools;
using WebCrawler.Presentation;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<CrawlProxies>();
        services.AddScoped<CrawlProducts>();
        services.AddScoped<ICrawlProxiesRepository, CrawlProxiesRepository>();
        services.AddScoped<ICrawlProductsRepository, CrawlProductsRepository>();
        services.AddScoped<IDirectoryCreator, DirectoryCreator>();
        services.AddScoped<IExportFiles, ExportFiles>();
        services.AddSingleton<IFileSystem, FileSystem>();
    })
    .Build();

var crawlProxies = host.Services.GetRequiredService<CrawlProxies>();
var crawlProducts = host.Services.GetRequiredService<CrawlProducts>();

await EntryPoint.UserInteraction(crawlProxies, crawlProducts);

await host.RunAsync();