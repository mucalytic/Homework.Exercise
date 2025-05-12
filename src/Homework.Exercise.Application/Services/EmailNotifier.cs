using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Options;
using Homework.Exercise.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reactive;
using FluentResults;
using MimeKit;

namespace Homework.Exercise.Application.Services;

public class EmailNotifier(ILogger<EmailNotifier> logger, IOptions<EmailSettings> emailSettings) : IPartnerNotifier
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public Result<Unit> Notify(IbtMessage message)
    {
        logger.LogInformation("Sending email notification for message with {EventType}.", message.EventType);
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(_emailSettings.RecipientName, _emailSettings.RecipientEmail));
            email.Subject = _emailSettings.Subject;
            email.Body = new TextPart("plain")
            {
                Text = $"Product Name: {message.ProductNameFull}\n" +
                       $"IBT Type Code: {message.IbtTypeCode}\n" +
                       $"Event Type: {message.EventType}\n" +
                       $"ISIN: {message.Isin}"
            };
            logger.LogInformation("Simulated email sent to Partner A for message with {EventType}.", message.EventType);
            return Result.Ok(Unit.Default);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email notification for message with {EventType}.", message.EventType);
            return Result.Fail<Unit>($"Failed to send email notification: {ex.Message}");
        }
    }
}
