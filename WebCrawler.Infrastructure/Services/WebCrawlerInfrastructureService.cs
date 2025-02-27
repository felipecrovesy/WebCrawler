using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using WebCrawler.Core.Entities;
using WebCrawler.Infrastructure.Data;

namespace WebCrawler.Infrastructure.Services;

public class WebCrawlerInfrastructureService
{
    public class CrawlerInfrastructureService : ICrawlerInfrastructureService
    {
        private readonly HttpClient _httpClient;
        private readonly IDbContextFactory<WebCrawlerContext> _contextFactory;
        
        public CrawlerInfrastructureService(HttpClient httpClient, IDbContextFactory<WebCrawlerContext> contextFactory)
        {
            _httpClient = httpClient;
            _contextFactory = contextFactory;
        }
        public async Task<List<ProxyInfo>> Crawl(string url)
        {
            var html = await _httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var proxyList = new List<ProxyInfo>();
            var table = doc.DocumentNode.SelectSingleNode("//table[@id='proxylisttable']");

            if (table != null)
            {
                var rows = table.SelectNodes("tbody/tr");
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes("td");
                        if (cells != null && cells.Count >= 8)
                        {
                            proxyList.Add(new ProxyInfo
                            {
                                IpAddress = cells[0].InnerText,
                                Port = int.Parse(cells[1].InnerText),
                                Country = cells[2].InnerText,
                                Protocol = cells[6].InnerText
                            });
                        }
                    }
                }
            }
            return proxyList;
        }

        public async Task<int> GetTotalPages(string url)
        {
          var html = await _httpClient.GetStringAsync(url);
          var doc = new HtmlDocument();
          doc.LoadHtml(html);

          //Find the last page Number
          var lastPageNode = doc.DocumentNode.SelectSingleNode("//a[@aria-label='Last']");

          if (lastPageNode != null) {
            var lastPageUrl = lastPageNode.GetAttributeValue("href", "");
            var uri = new Uri(lastPageUrl);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

            if (query["page"] != null) {
                return int.Parse(query["page"]);
            }
          }
          return 0;
        }
    }
}