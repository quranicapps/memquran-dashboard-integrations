using Integrations.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Integrations.Infrastructure.Configuration;

public class EventConfiguration : IEntityTypeConfiguration<EventAggregation>
{
    public void Configure(EntityTypeBuilder<EventAggregation> builder)
    {
        builder.ToTable(nameof(EventAggregation));
        
        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);
        
        builder.Property(p => p.LastModifiedBy)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}