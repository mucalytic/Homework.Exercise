using FluentResults;

namespace Homework.Exercise.Domain.Interfaces;

public interface IPathResolver
{
    Result<string> ResolvePath(string relativePath);
}
