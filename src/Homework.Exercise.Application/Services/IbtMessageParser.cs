using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Homework.Exercise.Application.Services;

public class IbtMessageParser(ILogger<IbtMessageParser> logger, IDateTimeProvider dateTimeProvider) : IIbtMessageParser
{
    private static XElement? ProductNameFull(XElement root) =>
        (from e1 in root.Elements()
         where e1.Name.LocalName == "Instrument"
         from e2 in e1.Elements()
         where e2.Name.LocalName == "ProductNameFull"
         select e2)
        .FirstOrDefault();

    private static XElement? IbtTypeCode(XElement root) =>
        (from e1 in root.Elements()
         where e1.Name.LocalName == "Instrument"
         from e2 in e1.Elements()
         where e2.Name.LocalName == "IBTTypeCode"
         select e2)
       .FirstOrDefault();

    private static IEnumerable<XElement> EventTypes(XElement root) =>
        from e1 in root.Elements()
        where e1.Name.LocalName == "Events"
        from e2 in e1.Elements()
        where e2.Name.LocalName == "Event"
        from e3 in e2.Elements()
        where e3.Name.LocalName == "EventType"
        select e3;
    
    public IEnumerable<IbtMessage> ParseMessages(string filePath)
    {
        logger.LogInformation("Parsing XML file {FilePath}.", filePath);
        try
        {
            var doc = XDocument.Load(filePath);
            if (doc.Root is null)
            {
                logger.LogWarning("Could not find root element of XML file {Root}", doc.Root);
                return [];
            }
            var ibtTypeCode = IbtTypeCode(doc.Root);
            var productNameFull = ProductNameFull(doc.Root);
            var isin = string.Empty; // there is no ISIN in the IBT.xml file.
            var messages = new List<IbtMessage>();
            var eventTypes = EventTypes(doc.Root);
            foreach (var eventType in eventTypes)
            {
                try
                {
                    var message = new IbtMessage(eventType.Value, productNameFull?.Value, ibtTypeCode?.Value, isin, dateTimeProvider.UtcNow);
                    messages.Add(message);
                    logger.LogInformation("Successfully parsed message with {EventType} from file {FilePath}.", eventType, filePath);
                }
                catch (ArgumentNullException ex)
                {
                    logger.LogError(ex, $"Failed to create {nameof(IbtMessage)}");
                }
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
