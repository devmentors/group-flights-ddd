using GroupFlights.Shared.Types;
using GroupFlights.TimeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.TimeManagement.Core.Data.EF.Configs;

internal class DeadlineNotificationConfiguration : IEntityTypeConfiguration<DeadlineNotification>
{
    public void Configure(EntityTypeBuilder<DeadlineNotification> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DueDate).IsRequired();
        builder.Property(x => x.State).IsRequired();
    }
}