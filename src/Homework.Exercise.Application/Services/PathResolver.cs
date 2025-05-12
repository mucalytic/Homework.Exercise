using Homework.Exercise.Domain.Interfaces;
using FluentResults;

namespace Homework.Exercise.Application.Services;

public class PathResolver : IPathResolver
{
    private readonly Lazy<DirectoryInfo?> _basePath = new(() =>
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null && directory.GetFiles("*.sln").Length == 0)
        {
            directory = directory.Parent;
        }
        return directory;
    });

    public Result<string> ResolvePath(string relativePath)
    {
        var basePath = _basePath.Value;
        return basePath is null
            ? Result.Fail<string>("Couldn't find base path.")
            : Path.Combine(basePath.FullName, relativePath).ToResult();
    }
}
