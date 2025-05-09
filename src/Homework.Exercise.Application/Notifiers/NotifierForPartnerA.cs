using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Models;
using System.Reactive;
using FluentResults;

namespace Homework.Exercise.Application.Notifiers;

public class NotifierForPartnerA : IPartnerNotifier
{
    public Result<Unit> Notify(IbtMessage ibtMessage)
    {
        throw new NotImplementedException();
    }
}
