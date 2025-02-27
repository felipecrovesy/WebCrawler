using WebCrawler.Core.Entities;

namespace WebCrawler.Core.Interfaces;

public interface IWebCrawlerService
{
    Task<List<ProxyInfo>> Crawl(string url);
    Task<int> GetTotalPages(string url);
}
