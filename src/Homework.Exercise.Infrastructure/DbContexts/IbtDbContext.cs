using Homework.Exercise.Infrastructure.ModelMappings;
using Homework.Exercise.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework.Exercise.Infrastructure.DbContexts;

public class IbtDbContext(DbContextOptions<IbtDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new IbtEventMapping());
    }
    
    public DbSet<IbtEvent> IbtEvents => Set<IbtEvent>();
}
