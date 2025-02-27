using Microsoft.EntityFrameworkCore;
using WebCrawler.Core.Entities;

namespace WebCrawler.Infrastructure.Data;

public class WebCrawlerContext : DbContext
{
    public WebCrawlerContext(DbContextOptions<WebCrawlerContext> options) : base(options) { }

    public DbSet<ExecutionLog> ExecutionLogs { get; set; }
}
