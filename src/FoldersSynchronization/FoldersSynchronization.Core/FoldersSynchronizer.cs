using Microsoft.Extensions.Logging;

namespace FoldersSynchronization.Core;

public class FoldersSynchronizer(ILogger<FoldersSynchronizer> logger) : IFoldersSynchronizer
{
    public async Task SynchronizeAsync(string source, string destination)
    {
        logger.LogInformation("Starting folders synchronization...");
        try
        {
            CleanUp(destination);
            await CopyFilesAsync(source, destination);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occured while synchronizing folders.");
            throw;
        }
        logger.LogInformation("Folders synchronization completed.");
    }

    private void CleanUp(string directory)
    {
        logger.LogInformation("Deleting folders and files from destination folder...");
        Parallel.ForEach(Directory.GetFiles(directory), File.Delete);
        Parallel.ForEach(Directory.GetDirectories(directory), dir => Directory.Delete(dir, true));
    }

    private async Task CopyFilesAsync(string source, string destination)
    {
        if (!Directory.Exists(source))
        {
            throw new DirectoryNotFoundException(source);
        }

        if (!Directory.Exists(destination))
        {
            throw new DirectoryNotFoundException(destination);
        }

        await Parallel.ForEachAsync(Directory.GetFiles(source),  async (sourceFile, ct) =>
        {
            var fileName = Path.GetFileName(sourceFile);
            var destinationFile = Path.Combine(destination, fileName);
            
            logger.LogInformation($"Copying file {sourceFile} to {destinationFile}.");
            await using var sourceStream = File.Open(sourceFile, FileMode.Open);
            await using var destinationStream = File.Create(destinationFile);
            await sourceStream.CopyToAsync(destinationStream, ct);
        });

        foreach (var sourceDirectory in Directory.GetDirectories(source))
        {
            var destinationDirectory = Path.Combine(destination, Path.GetFileName(sourceDirectory));
            logger.LogInformation($"Copying directory {sourceDirectory} to {destinationDirectory}.");
            Directory.CreateDirectory(destinationDirectory);
            await CopyFilesAsync(sourceDirectory, destinationDirectory);
        }
    }
}