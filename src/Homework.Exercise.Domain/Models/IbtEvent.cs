namespace Homework.Exercise.Domain.Models;

public class IbtEvent
{
    public IbtEvent() { }

    public IbtEvent(string eventType, DateTime timeStamp)
    {
        EventType = eventType;
        TimeStamp = timeStamp;
    }

    public int      Id        { get; init; }
    public string   EventType { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
}
