namespace Homework.Exercise.Domain.Options;

public class EmailSettings
{
    public string Subject        { get; init; } = string.Empty;
    public string SenderName     { get; init; } = string.Empty;
    public string SenderEmail    { get; init; } = string.Empty;
    public string RecipientName  { get; init; } = string.Empty;
    public string RecipientEmail { get; init; } = string.Empty;
}
