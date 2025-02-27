using System.IO.Abstractions.TestingHelpers;
using WebCrawler.Infrastructure.Tools;

namespace WebCrawler.UnitTests
{
    public class DirectoryCreatorTests
    {
        [Fact]
        public async Task Given_CreateDirectory_When_DirectoryDoesNotExist_Then_CreatesDirectory()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var directoryCreator = new DirectoryCreator(mockFileSystem);
            var directoryPath = "/test/directory";
            var filePath = "/test/directory/file.txt";

            // Act
            await directoryCreator.Create(directoryPath, filePath);

            // Assert
            Assert.True(mockFileSystem.Directory.Exists(directoryPath));
        }

        [Fact]
        public async Task Given_CreateDirectory_When_DirectoryExists_Then_DoesNotCreateDirectoryAgain()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var directoryPath = "/test/directory";
            var filePath = "/test/directory/file.txt";
            mockFileSystem.AddDirectory(directoryPath);
            var directoryCreator = new DirectoryCreator(mockFileSystem);

            // Act
            await directoryCreator.Create(directoryPath, filePath);

            // Assert
            Assert.True(mockFileSystem.Directory.Exists(directoryPath));
        }
    }
}