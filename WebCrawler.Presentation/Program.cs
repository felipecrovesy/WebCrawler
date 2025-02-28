using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCrawler.Core.Ports;
using WebCrawler.Core.UseCases.CrawlProxies;
using WebCrawler.Infrastructure.Repositories;
using WebCrawler.Infrastructure.Tools;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<CrawlProxiesUseCase>();
        services.AddScoped<ICrawlProxiesRepository, CrawlProxiesRepository>();
        services.AddScoped<IDirectoryCreator, DirectoryCreator>();
        services.AddScoped<IExportFiles, ExportFiles>();
        services.AddSingleton<IFileSystem, FileSystem>();
    })
    .Build();

var crawlProxiesUseCase = host.Services.GetRequiredService<CrawlProxiesUseCase>();

await crawlProxiesUseCase.Handle();

await host.RunAsync();