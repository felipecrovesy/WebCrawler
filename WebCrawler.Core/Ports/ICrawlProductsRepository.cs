namespace WebCrawler.Core.Ports
{
    public interface ICrawlProductsRepository
    {
        Task Save(DateTime initialDate, DateTime finalDate, int pageNumbers, int numberLines, string JsonFile, string pageSource, string dbPath);
    }
}