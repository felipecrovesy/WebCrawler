namespace WebCrawler.Core.Entities;

public class ExecutionLog
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int PageCount { get; set; }
    public int LineCount { get; set; }
    public string JsonFilePath { get; set; }
}