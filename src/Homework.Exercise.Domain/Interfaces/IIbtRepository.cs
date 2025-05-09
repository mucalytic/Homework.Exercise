using Homework.Exercise.Domain.Models;
using FluentResults;

namespace Homework.Exercise.Domain.Interfaces;

public interface IIbtRepository
{
    Task<Result<IbtEvent>> CreateIbtEventAsync(EventType eventType, CancellationToken token);
}
