using Homework.Exercise.Infrastructure.DbContexts;
using Homework.Exercise.Domain.Interfaces;
using Homework.Exercise.Domain.Models;
using Microsoft.EntityFrameworkCore;
using FluentResults;

namespace Homework.Exercise.Infrastructure.Repositories;

public class IbtRepository : IIbtRepository, IDisposable, IAsyncDisposable
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IbtDbContext _dbContext;

    public IbtRepository(DbContextOptions<IbtDbContext> options, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        _dbContext = new IbtDbContext(options ?? throw new ArgumentNullException(nameof(options)));
    }
    
    public void Dispose() =>
        _dbContext.Dispose();

    public async ValueTask DisposeAsync() =>
        await _dbContext.DisposeAsync();
    
    public async Task<Result<IbtEvent>> CreateIbtEventAsync(EventType eventType, CancellationToken token)
    {
        var ibtEvent = new IbtEvent(eventType.Value, _dateTimeProvider.UtcNow);
        try
        {
            await _dbContext.IbtEvents.AddAsync(ibtEvent, token).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(token).ConfigureAwait(false);
            return ibtEvent.ToResult();
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail($"Database error: {ex.InnerException?.Message ?? ex.Message}");
        }
        catch (OperationCanceledException)
        {
            return Result.Fail("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Unexpected error: {ex.Message}");
        }
    }
}
