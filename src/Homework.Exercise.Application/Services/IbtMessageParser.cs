using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Homework.Exercise.Application.Services;

public class IbtMessageParser(ILogger<IbtMessageParser> logger, IDateTimeProvider dateTimeProvider) : IIbtMessageParser
{
    public IEnumerable<IbtMessage> ParseMessages(string filePath)
    {
        logger.LogInformation("Parsing XML file {FilePath}.", filePath);
        try
        {
            var messages = new List<IbtMessage>();
            var doc = XDocument.Load(filePath);
            var eventType = doc.Descendants("EventType").FirstOrDefault()?.Value; // TODO: should look for event types and create message for each one
            var productNameFull = doc.Descendants("ProductNameFull").FirstOrDefault()?.Value;
            var ibtTypeCode = doc.Descendants("IBTTypeCode").FirstOrDefault()?.Value;
            var isin = doc.Descendants("ISIN").FirstOrDefault(e => e.Attribute("IdSchemeCode")?.Value == "-I")?.Value;
            try
            {
                var message = new IbtMessage(eventType, productNameFull, ibtTypeCode, isin, dateTimeProvider.UtcNow);
                messages.Add(message);
                logger.LogInformation("Successfully parsed message with {EventType} from file {FilePath}.", eventType, filePath);
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(ex, $"Failed to create {nameof(IbtMessage)}");
            }
            return messages;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse XML file {FilePath}.", filePath);
            throw;
        }
    }
}
