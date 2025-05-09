using Homework.Exercise.Infrastructure.Repositories;
using Homework.Exercise.Infrastructure.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Homework.Exercise.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homework.Exercise.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IIbtRepository>(provider =>
            new IbtRepository(new DbContextOptionsBuilder<IbtDbContext>().Options,
            provider.GetRequiredService<IDateTimeProvider>()));
        return services;
    }    
}
