namespace Homework.Exercise.Domain.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
