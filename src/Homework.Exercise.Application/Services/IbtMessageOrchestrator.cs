using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Options;
using Homework.Exercise.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Homework.Exercise.Application.Services;

public class IbtMessageOrchestrator(
    ILogger<IbtMessageOrchestrator> logger,
    IOptions<FileSettings> fileSettings,
    IIbtMessageParser messageParser,
    IPartnerNotifier[] notifiers,
    IIbtRepository ibtRepository,
    IFileReader fileReader) : IIbtMessageOrchestrator
{
    private readonly FileSettings _fileSettings = fileSettings.Value;
    
    public async Task ProcessMessagesAsync(CancellationToken token)
    {
        var filePaths = fileReader.GetFilePaths().ToArray();
        if (filePaths.Length == 0)
        {
            logger.LogInformation("No files found in inbox directory.");
            return;
        }
        logger.LogInformation("Found {FileCount} files to process.", filePaths.Length);
        foreach (var filePath in filePaths)
        {
            logger.LogInformation("Processing file {FilePath}.", filePath);
            try
            {
                var messages = messageParser.ParseMessages(filePath).ToArray();
                logger.LogInformation("Parsed {MessageCount} messages from file {FilePath}.", messages.Length, filePath);
                foreach (var message in messages)
                {
                    try
                    {
                        await ibtRepository.CreateIbtEventAsync(new EventType(message.EventType), token);
                        logger.LogInformation("Saved IbtEvent to database: {EventType}, {Timestamp}.",
                            message.EventType, message.Timestamp);
                    }
                    catch (ArgumentNullException ex)
                    {
                        logger.LogError(ex, $"Failed to create {nameof(EventType)}");
                        continue;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to save IbtEvent to database for message with {EventType}.",
                            message.EventType);
                        continue;
                    }
                    foreach (var notifier in notifiers)
                    {
                        var notifierType = notifier.GetType().Name;
                        try
                        {
                            var result = notifier.Notify(message);
                            if (result.IsSuccess)
                            {
                                logger.LogInformation("Successfully notified {NotifierType} for message with {EventType}.",
                                    notifierType, message.EventType);
                            }
                            else
                            {
                                logger.LogWarning("Notification failed for {NotifierType}: {Error}. {EventType}.",
                                    notifierType, result.Errors.First().Message, message.EventType);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Unexpected error while notifying {NotifierType} for message with {EventType}.",
                                notifierType, message.EventType);
                        }
                    }
                }
                var archivePath = Path.Combine(_fileSettings.ArchivePath, Path.GetFileName(filePath));
                try
                {
                    File.Move(filePath, archivePath, overwrite: true);
                    logger.LogInformation("Moved processed file from {FilePath} to {ArchivePath}.", filePath, archivePath);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to move file from {FilePath} to {ArchivePath}.", filePath, archivePath);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process file {FilePath}.", filePath);
            }
        }
    }
}
