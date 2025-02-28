using WebCrawler.Core.UseCases.CrawlEcommerce;
using WebCrawler.Core.UseCases.CrawlProxies;

namespace WebCrawler.Presentation;

public static class EntryPoint
{
    public static async Task UserInteraction(CrawlProxies crawlProxies, CrawlProducts crawlProducts)
    {
        while (true)
        {
            DrawMenu(["1 - Crawl Proxies", " 2 - Crawl Products"]);

            Console.Write("Escolha o item que deseja coletar: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await crawlProxies.Handle();
                    break;
                case "2":
                    await crawlProducts.Handle();
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            Console.WriteLine("\nPressione Enter para continuar...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private static void DrawMenu(string[] options)
    {
        var title = "Welcome to Crawler!";

        var maxWidth = Math.Max(title.Length, options.Max(option => option.Length)) + 4;

        DrawTopLines(maxWidth);
        DrawCenteredText(title, maxWidth);
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
            DrawCenteredText(option, width);
        }
    }

    private static void DrawCenteredText(string text, int width)
    {
        var padding = width - text.Length - 2;
        var leftPadding = padding / 2;
        var rightPadding = padding - leftPadding;

        Console.WriteLine("|" + new string(' ', leftPadding) + text + new string(' ', rightPadding) + "|");
    }
}