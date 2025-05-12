using Homework.Exercise.Domain.Models;

namespace Homework.Exercise.Domain.Interfaces;

public interface IIbtMessageParser
{
    IEnumerable<IbtMessage> ParseMessages(string filePath);
}
