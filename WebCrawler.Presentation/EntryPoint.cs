using WebCrawler.Core.UseCases.CrawlEcommerce;
using WebCrawler.Core.UseCases.CrawlProxies;

namespace WebCrawler.Presentation;

public class EntryPoint
{
    public static async Task UserInteraction(CrawlProxies crawlProxies, CrawlProducts crawlProducts)
    {
        DrawMenu(["1 - CrawlProxies", "2 - CrawlProducts"]);

        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                await crawlProxies.Handle();
                break;
            case "2":
                await crawlProducts.Handle();
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    private static void DrawMenu(string[] options)
    {
        var maxWidth = options.Max(option => option.Length) + 4;

        DrawTopLines(maxWidth);
        DrawOptions(options, maxWidth);
        DrawBottomLines(maxWidth);
    }

    private static void DrawTopLines(int width)
    {
        Console.WriteLine("+" + new string('-', width - 2) + "+");
    }

    private static void DrawBottomLines(int width)
    {
        Console.WriteLine("+" + new string('-', width - 2) + "+");
    }

    private static void DrawOptions(string[] options, int width)
    {
        foreach (var option in options)
        {
            var leftPadding = (width - option.Length - 2) / 2;
            var rightPadding = width - option.Length - 2 - leftPadding;

            Console.WriteLine("|" + new string(' ', leftPadding) + option + new string(' ', rightPadding) + "|");
        }
    }
}