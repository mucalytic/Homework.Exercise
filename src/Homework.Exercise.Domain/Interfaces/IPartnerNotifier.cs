using Homework.Exercise.Domain.Models;
using System.Reactive;
using FluentResults;

namespace Homework.Exercise.Domain.Interfaces;

public interface IPartnerNotifier
{
    Result<Unit> Notify(IbtMessage ibtMessage);
}
