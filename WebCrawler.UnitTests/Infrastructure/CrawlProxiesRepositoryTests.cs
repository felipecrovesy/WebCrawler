using Moq;
using WebCrawler.Core.Ports;

namespace WebCrawler.UnitTests.Infrastructure
{
    public class CrawlProductsRepositoryTests
    {
        [Fact]
        public async Task Save_ShouldSaveDataCorrectly()
        {
            // Arrange
            var mockRepository = new Mock<ICrawlProductsRepository>();
            DateTime capturedInitialDate = default;
            DateTime capturedFinalDate = default;
            int capturedPageNumbers = default;
            int capturedNumberLines = default;
            string capturedJsonFile = null;
            string capturedPageSource = null;
            string capturedDbPath = null;

            mockRepository.Setup(repo => repo.Save(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<DateTime, DateTime, int, int, string, string, string>((initialDate, finalDate, pageNumbers, numberLines, jsonFile, pageSource, dbPath) =>
                {
                    capturedInitialDate = initialDate;
                    capturedFinalDate = finalDate;
                    capturedPageNumbers = pageNumbers;
                    capturedNumberLines = numberLines;
                    capturedJsonFile = jsonFile;
                    capturedPageSource = pageSource;
                    capturedDbPath = dbPath;
                })
                .Returns(Task.CompletedTask);

            var initialDate = DateTime.Now;
            var finalDate = DateTime.Now.AddHours(1);
            var pageNumbers = 1;
            var numberLines = 10;
            var jsonFilePath = Path.Combine("jsonFileForTest", "product_data_page_1_20250228_041245.json");
            var jsonFile = await File.ReadAllTextAsync(jsonFilePath);
            var pageSource = "<html></html>";
            var dbPath = "path/to/db";

            // Act
            await mockRepository.Object.Save(initialDate, finalDate, pageNumbers, numberLines, jsonFile, pageSource, dbPath);

            // Assert
            Assert.Equal(initialDate, capturedInitialDate);
            Assert.Equal(finalDate, capturedFinalDate);
            Assert.Equal(pageNumbers, capturedPageNumbers);
            Assert.Equal(numberLines, capturedNumberLines);
            Assert.Equal(jsonFile, capturedJsonFile);
            Assert.Equal(pageSource, capturedPageSource);
            Assert.Equal(dbPath, capturedDbPath);
        }
    }
}