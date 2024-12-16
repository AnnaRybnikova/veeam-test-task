using FoldersSynchronization.Core;
using Xunit.Abstractions;

namespace FoldersSynchronization.Tests;

public class Tests(ITestOutputHelper output) : IDisposable
{
    private readonly DirectoryInfo _source = Directory.CreateDirectory("source");
    private readonly DirectoryInfo _destination = Directory.CreateDirectory("destination");
    private readonly IFoldersSynchronizer _foldersSynchronizer = new FoldersSynchronizer(new TestOutputLogger<FoldersSynchronizer>(output));

    [Fact]
    public async Task ShouldSynchronizeFilesAndDirectories()
    {
        // Arrange
        PrepareSourceDirectory();
        PrepareDestinationDirectory();

        // Act
        await _foldersSynchronizer.SynchronizeAsync(_source.FullName, _destination.FullName);

        // Assert
        CompareSourceAndDestination();
    }

    private void PrepareSourceDirectory()
    {
        File.WriteAllText( Path.Combine(_source.FullName, "file1.txt"), "Hello World! (1)");
        File.WriteAllText(Path.Combine(_source.FullName, "file2.txt"), "Hello World! (2)");
        Directory.CreateDirectory(Path.Combine(_source.FullName, "subDirectory"));
    }
    
    private void PrepareDestinationDirectory()
    {
        File.WriteAllText( Path.Combine(_destination.FullName, "file4.txt"), "Hello World! (4)");
    }

    private void CompareSourceAndDestination()
    {
        CompareDirectories(_source.FullName, _destination.FullName);
    }

    private void CompareFiles(string left, string right)
    {
        var leftText = File.ReadAllText(left);
        var rightText = File.ReadAllText(right);
        
        Assert.Equal(leftText, rightText);
    }

    private void CompareDirectories(string left, string right)
    {
        var leftFiles = Directory.GetFiles(left).OrderBy(s => s).ToArray();
        var rightFiles = Directory.GetFiles(right).OrderBy(s => s).ToArray();
        
        Assert.Equal(leftFiles.Select(Path.GetFileName), rightFiles.Select(Path.GetFileName));
        foreach (var (leftFile, rightFile) in leftFiles.Zip(rightFiles))
        {
            CompareFiles(leftFile, rightFile);
        }

        var leftDirectories = Directory.GetDirectories(left).OrderBy(s => s).ToArray();
        var rightDirectories = Directory.GetDirectories(right).OrderBy(s => s).ToArray();
        Assert.Equal(leftDirectories.Select(Path.GetFileName), rightDirectories.Select(Path.GetFileName));
        
        foreach (var (leftDir, rightDir) in leftDirectories.Zip(rightDirectories))
        {
            CompareDirectories(leftDir, rightDir);
        }
    }

    public void Dispose()
    {
        Directory.Delete(_source.FullName, true);
        Directory.Delete(_destination.FullName, true);
    }
}