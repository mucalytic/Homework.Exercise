namespace Homework.Exercise.Domain.Models;

public class IbtMessage
{
    public IbtMessage(
        string? eventType,
        string? productNameFull,
        string? ibtTypeCode,
        string? isin,
        DateTime timestamp)
    {
        ArgumentNullException.ThrowIfNull(productNameFull);
        ArgumentNullException.ThrowIfNull(ibtTypeCode);
        ArgumentNullException.ThrowIfNull(eventType);
        ArgumentNullException.ThrowIfNull(isin);
        ProductNameFull = productNameFull;
        IbtTypeCode = ibtTypeCode;
        EventType = eventType;
        Timestamp = timestamp;
        Isin = isin;
    }

    public string   EventType       { get; }
    public string   ProductNameFull { get; }
    public string   IbtTypeCode     { get; }
    public string   Isin            { get; }
    public DateTime Timestamp       { get; }
}
