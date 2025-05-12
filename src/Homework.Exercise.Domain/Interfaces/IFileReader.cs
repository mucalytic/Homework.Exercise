namespace Homework.Exercise.Domain.Interfaces;

public interface IFileReader
{
    IEnumerable<string> GetFilePaths();
}
