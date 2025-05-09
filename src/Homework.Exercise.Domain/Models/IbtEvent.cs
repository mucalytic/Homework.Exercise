namespace Homework.Exercise.Domain.Models;

public class IbtEvent
{
    public int      Id        { get; set; }
    public string   EventType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
