using WebCrawler.Core.Entities;

namespace WebCrawler.Core.Interfaces;

public interface IProxyRepository
{
    void SaveExecutionLog(ExecutionLog log);
}