namespace Homework.Exercise.Domain.Interfaces;

public interface IIbtMessageOrchestrator
{
    Task ProcessMessagesAsync(CancellationToken token);
}
