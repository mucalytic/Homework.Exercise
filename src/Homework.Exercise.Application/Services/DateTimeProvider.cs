using Homework.Exercise.Domain.Interfaces;

namespace Homework.Exercise.Application.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
