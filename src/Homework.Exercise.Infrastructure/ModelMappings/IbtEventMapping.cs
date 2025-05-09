using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Homework.Exercise.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework.Exercise.Infrastructure.ModelMappings;

public class IbtEventMapping : IEntityTypeConfiguration<IbtEvent>
{
    public void Configure(EntityTypeBuilder<IbtEvent> builder)
    {
        builder.HasKey(ibtEvent => ibtEvent.Id);
        builder.Property(ibtEvent => ibtEvent.EventType)
               .IsRequired()
               .HasMaxLength(16);
        builder.Property(ibtEvent => ibtEvent.TimeStamp)
               .IsRequired();
    }
}
