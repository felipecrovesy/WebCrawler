using WebCrawler.Core.Entities;
using WebCrawler.Core.Interfaces;
using WebCrawler.Infrastructure.Data;

namespace WebCrawler.Infrastructure.Repositories;

public class ProxyRepository : IProxyRepository
{
    private readonly WebCrawlerContext _context;

    public ProxyRepository(WebCrawlerContext context)
    {
        _context = context;
    }

    public void SaveExecutionLog(ExecutionLog log)
    {
        _context.ExecutionLogs.Add(log);
        _context.SaveChanges();
    }
}
