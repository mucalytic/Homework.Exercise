using Homework.Exercise.Application.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Homework.Exercise.Application.Services;
using Homework.Exercise.Domain.Interfaces;

namespace Homework.Exercise.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPartnerNotifier, FileNotifier>();
        services.AddSingleton<IPartnerNotifier, EmailNotifier>();
        services.AddSingleton<IFileReader, FileReader>();
        services.AddSingleton<IPathResolver, PathResolver>();
        services.AddSingleton<IIbtMessageParser, IbtMessageParser>();
        services.AddSingleton<IIbtMessageOrchestrator, IbtMessageOrchestrator>();
        services.AddHostedService<IbtMessageHost>();
        return services;
    }
}
