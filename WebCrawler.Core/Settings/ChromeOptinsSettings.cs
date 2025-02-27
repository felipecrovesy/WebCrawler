using OpenQA.Selenium.Chrome;

namespace WebCrawler.Core.Settings;

public static class ChromeOptionsSettings
{
    public static ChromeOptions ChromeOptions()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless");

        return chromeOptions;
    }
}