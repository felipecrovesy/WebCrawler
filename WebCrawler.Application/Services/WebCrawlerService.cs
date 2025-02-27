using WebCrawler.Core.Entities;
using WebCrawler.Core.Interfaces;
using WebCrawler.Infrastructure.Services;

namespace WebCrawler.Application.Services
{
    public class WebCrawlerService : IWebCrawlerService
    {
        private readonly ICrawlerInfrastructureService _webCrawlerInfrastructureService;

        public WebCrawlerService(ICrawlerInfrastructureService webCrawlerInfrastructureService)
        {
            _webCrawlerInfrastructureService = webCrawlerInfrastructureService;
        }

        public async Task<List<ProxyInfo>> Crawl(string url)
        {
            return await _webCrawlerInfrastructureService.Crawl(url);
        }

        public async Task<int> GetTotalPages(string url)
        {
            return await _webCrawlerInfrastructureService.GetTotalPages(url);
        }
    }
}