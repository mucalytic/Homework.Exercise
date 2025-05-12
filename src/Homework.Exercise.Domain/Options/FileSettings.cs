namespace Homework.Exercise.Domain.Options;

public class FileSettings
{
    public string InboxPath   { get; init; } = string.Empty;
    public string OutboxPath  { get; init; } = string.Empty;
    public string ArchivePath { get; init; } = string.Empty;
    public string FilePattern { get; init; } = string.Empty;
}
