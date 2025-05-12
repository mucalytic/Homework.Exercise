using Microsoft.Extensions.DependencyInjection;
using Homework.Exercise.Domain.Options;
using Microsoft.Extensions.Hosting;

namespace Homework.Exercise.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, HostBuilderContext ctx)
    {
        services.AddOptions<FileSettings>()
                .Bind(ctx.Configuration.GetSection(nameof(FileSettings)));
        services.AddOptions<EmailSettings>()
                .Bind(ctx.Configuration.GetSection(nameof(EmailSettings)));
        services.AddOptions<FileNotifierSettings>()
                .Bind(ctx.Configuration.GetSection(nameof(FileNotifierSettings)));
        return services;
    }    
}
