using GroupFlights.Sales.Application.DeadlineRegistry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Sales.Infrastructure.Data.EF.Configs;

internal class DeadlineRegistryEntryConfiguration : IEntityTypeConfiguration<DeadlineRegistryEntry>
{
    public void Configure(EntityTypeBuilder<DeadlineRegistryEntry> builder)
    {
        builder.HasKey(x => x.DeadlineId);
        builder.Property(x => x.SourceId).IsRequired();
        builder.Property(x => x.SourceType).IsRequired();
    }
}