using GroupFlights.Shared.Types;
using GroupFlights.TimeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.TimeManagement.Core.Data.EF.Configs;

internal class DeadlineParticipantConfiguration : IEntityTypeConfiguration<DeadlineParticipant>
{
    public void Configure(EntityTypeBuilder<DeadlineParticipant> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId)
            .IsRequired(false);
        
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired(false);
    }
}