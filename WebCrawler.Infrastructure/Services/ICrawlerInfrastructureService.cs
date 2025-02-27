using WebCrawler.Core.Entities;

namespace WebCrawler.Infrastructure.Services;

public interface ICrawlerInfrastructureService
{
    Task<List<ProxyInfo>> Crawl(string url);
    Task<int> GetTotalPages(string url);
}