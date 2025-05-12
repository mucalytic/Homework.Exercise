using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Exercise.Application.Services;

public class FileReader(
    IPathResolver pathResolver,
    ILogger<FileReader> logger,
    IOptions<FileSettings> fileSettings) : IFileReader
{
    private readonly FileSettings _fileSettings = fileSettings.Value;

    public IEnumerable<string> GetFilePaths()
    {
        var inboxPathResult = pathResolver.ResolvePath(_fileSettings.InboxPath);
        if (inboxPathResult.IsFailed || !Directory.Exists(inboxPathResult.Value))
        {
            logger.LogWarning("Inbox directory {InboxPath} does not exist.", _fileSettings.InboxPath);
            return [];
        }
        var files = Directory.GetFiles(inboxPathResult.Value, _fileSettings.FilePattern);
        logger.LogInformation("Found {FileCount} XML files in {InboxPath}.", files.Length, inboxPathResult.Value);
        return files;
    }
}
