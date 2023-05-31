using GroupFlights.Postsale.Application.Queries.GetChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Postsale.Infrastructure.Data.EF.Configs;

internal class ChangeRequestBasicDataConfig : IEntityTypeConfiguration<ChangeRequestBasicData>
{
    public void Configure(EntityTypeBuilder<ChangeRequestBasicData> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NewTravelDate).HasColumnName("_newTravelDate");
        builder.Property(x => x.ReservationId).HasColumnName("_reservationToChangeReservationId");
        builder.Property(x => x.RequesterId).HasColumnName("_requester");
        builder.Property(x => x.CompletionStatus).HasColumnName("_completionStatus").IsRequired(false);

        builder.ToTable("ChangeRequests");
    }
}