using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Exercise.Application.Services;

public class FileReader(ILogger<FileReader> logger, IOptions<FileSettings> fileSettings) : IFileReader
{
    private readonly FileSettings _fileSettings = fileSettings.Value;

    public IEnumerable<string> GetFilePaths()
    {
        if (!Directory.Exists(_fileSettings.InboxPath))
        {
            logger.LogWarning("Inbox directory {InboxPath} does not exist.", _fileSettings.InboxPath);
            return [];
        }
        var files = Directory.GetFiles(_fileSettings.InboxPath, _fileSettings.FilePattern);
        logger.LogInformation("Found {FileCount} XML files in {InboxPath}.", files.Length, _fileSettings.InboxPath);
        return files;
    }
}
