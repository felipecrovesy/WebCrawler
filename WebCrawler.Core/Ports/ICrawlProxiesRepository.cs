namespace WebCrawler.Core.Ports
{
    public interface ICrawlProxiesRepository
    {
        Task Save(DateTime initialDate, DateTime finalDate, int pageNumbers, int numberLines, string JsonFile, string pageSource);
    }
}