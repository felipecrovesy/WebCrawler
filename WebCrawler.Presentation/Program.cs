using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCrawler.Core.Ports;
using WebCrawler.Core.UseCases.CrawlProxies;
using WebCrawler.Infrastructure.Repositories;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<CrawlProxiesUseCase>();
        services.AddTransient<ICrawlProxiesRepository, CrawlProxiesRepository>();
    })
    .Build();

var crawlProxiesUseCase = host.Services.GetRequiredService<CrawlProxiesUseCase>();

await crawlProxiesUseCase.Handle();

await host.RunAsync();