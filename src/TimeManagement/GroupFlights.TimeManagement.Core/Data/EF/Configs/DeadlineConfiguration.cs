using GroupFlights.TimeManagement.Core.Models;
using GroupFlights.TimeManagement.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.TimeManagement.Core.Data.EF.Configs;

internal class DeadlineConfiguration : IEntityTypeConfiguration<Deadline>
{
    public void Configure(EntityTypeBuilder<Deadline> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new DeadlineId(x))
            .IsRequired();
        
        builder.Property(x => x.CommunicationChannel).IsRequired();
        builder.OwnsOne(x => x.Message);
        builder.Property(x => x.DeadlineDateUtc).IsRequired();
        builder.Property(x => x.Fulfilled).IsRequired(false);

        builder.HasMany(x => x.Participants);
        builder.HasMany(x => x.Notifications);
    }
}