using static Homework.Exercise.Domain.Errors.Messages;

namespace Homework.Exercise.Domain.Models;

public class EventType
{
    public EventType(string? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length > 16) throw new ArgumentException(MaxStringLengthIs(16), nameof(value));
        Value = value;
    }

    public string Value { get; }
}
