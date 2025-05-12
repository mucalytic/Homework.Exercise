using Homework.Exercise.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Homework.Exercise.Application.HostedServices;

public class IbtMessageHost(
    IIbtMessageOrchestrator orchestrator,
    IDateTimeProvider dateTimeProvider,
    ILogger<IbtMessageHost> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{ServiceName} started. Processing messages every minute.", nameof(IbtMessageHost));
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Processing messages at {Timestamp}", dateTimeProvider.UtcNow);
            await orchestrator.ProcessMessagesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
        logger.LogInformation("{ServiceName} stopped", nameof(IbtMessageHost));
    }
}
