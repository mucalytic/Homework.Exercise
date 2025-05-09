namespace Homework.Exercise.Domain.Models;

public record IbtMessage(
    string   EventType,
    string   ProductNameFull,
    string   IbtTypeCode,
    string   Isin,
    DateTime Timestamp
);
