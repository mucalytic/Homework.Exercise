using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Options;
using Homework.Exercise.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reactive;
using System.Xml.Linq;
using FluentResults;

namespace Homework.Exercise.Application.Services;

public class FileNotifier(
    ILogger<FileNotifier> logger,
    IOptions<FileSettings> fileSettings,
    IOptions<FileNotifierSettings> fileNotifierSettings) : IPartnerNotifier
{
    private readonly FileNotifierSettings _fileNotifierSettings = fileNotifierSettings.Value;
    private readonly FileSettings _fileSettings = fileSettings.Value;
    
    public Result<Unit> Notify(IbtMessage message)
    {
        logger.LogInformation("Processing file notification for message with {EventType}.", message.EventType);
        if (message.EventType != _fileNotifierSettings.RequiredEventType || string.IsNullOrEmpty(message.Isin))
        {
            logger.LogInformation("Skipping file notification: {EventType}, {ISIN}.", message.EventType, message.Isin);
            return Result.Ok(Unit.Default);
        }
        try
        {
            var xml = new XElement("InstrumentNotification",
                new XElement("Timespan", message.Timestamp.ToString("o")),
                new XElement("ISIN", message.Isin));
            var fileName = string.Format(_fileNotifierSettings.OutputFileNameFormat, message.Timestamp.Ticks);
            var filePath = Path.Combine(_fileSettings.OutboxPath, fileName);
            if (!Directory.Exists(_fileSettings.OutboxPath)) Directory.CreateDirectory(_fileSettings.OutboxPath);
            xml.Save(filePath);
            logger.LogInformation("Saved InstrumentNotification to {FilePath} for message with {EventType}.", filePath, message.EventType);
            return Result.Ok(Unit.Default);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save InstrumentNotification for message with {EventType}.", message.EventType);
            return Result.Fail<Unit>($"Failed to save InstrumentNotification: {ex.Message}");
        }
    }
}
