using System.IO.Abstractions.TestingHelpers;
using WebCrawler.Infrastructure.Tools;

namespace WebCrawler.UnitTests
{
    public class ExportFilesTests
    {
        [Fact]
        public async Task Given_WriteFile_When_FileDoesNotExist_Then_CreatesFile()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddDirectory("/test/directory");
            var exportFiles = new ExportFiles(mockFileSystem);
            var filePath = "/test/directory/file.txt";
            var content = "Test content";

            // Act
            await exportFiles.WriteFile(filePath, content);

            // Assert
            Assert.True(mockFileSystem.File.Exists(filePath));
            Assert.Equal(content, mockFileSystem.File.ReadAllText(filePath));
        }

        [Fact]
        public async Task Given_WriteFile_When_FileExists_Then_OverwritesFile()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var filePath = "/test/directory/file.txt";
            var initialContent = "Initial content";
            var newContent = "New content";
            mockFileSystem.AddFile(filePath, new MockFileData(initialContent));
            var exportFiles = new ExportFiles(mockFileSystem);

            // Act
            await exportFiles.WriteFile(filePath, newContent);

            // Assert
            Assert.True(mockFileSystem.File.Exists(filePath));
            Assert.Equal(newContent, mockFileSystem.File.ReadAllText(filePath));
        }
    }
}