using Microsoft.Extensions.DependencyInjection;
using Homework.Exercise.Application.Notifiers;
using Homework.Exercise.Application.Services;
using Homework.Exercise.Domain.Interfaces;

namespace Homework.Exercise.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPartnerNotifier, NotifierForPartnerA>();
        services.AddSingleton<IPartnerNotifier, NotifierForPartnerB>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
