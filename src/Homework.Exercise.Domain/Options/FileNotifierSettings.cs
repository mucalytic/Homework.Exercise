namespace Homework.Exercise.Domain.Options;

public class FileNotifierSettings
{
    public string RequiredEventType    { get; init; } = string.Empty;
    public string OutputFileNameFormat { get; init; } = string.Empty;
}