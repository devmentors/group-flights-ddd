using GroupFlights.Postsale.Domain.Changes.Outcome;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Postsale.Infrastructure.Data.EF.Configs;

internal class ReservationChangeToApplyConfig : IEntityTypeConfiguration<ReservationChangeToApply>
{
    public void Configure(EntityTypeBuilder<ReservationChangeToApply> builder)
    {
        builder.HasKey(x => x.Id);
        builder.OwnsOne<TravelChange>("_travelTravelChange", tc =>
        {
            tc.OwnsMany(x => x.NewTravelSegments, t =>
            {
                t.OwnsOne(x => x.FlightTime);
                t.Property(x => x.Date).IsRequired();
                t.OwnsOne(x => x.SourceAirport, a =>
                {
                    a.Property(x => x.Code)
                        .HasConversion(x => x.Value,
                            x => new(x))
                        .IsRequired();
                });
                t.OwnsOne(x => x.TargetAirport, a =>
                {
                    a.Property(x => x.Code)
                        .HasConversion(x => x.Value,
                            x => new(x))
                        .IsRequired();
                });
                t.OwnsOne(x => x.FlightTime);
            });
        });

        builder.OwnsOne<ReservationCost>("_costAfterChange",
            c =>
            {
                c.OwnsOne(x => x.TotalCost, m =>
                {
                    m.Property(x => x.Amount).IsRequired();
                    m.Property(x => x.Currency).IsRequired();
                });
                c.Navigation(x => x.TotalCost).IsRequired();
                c.OwnsOne(x => x.RefundableCost, m =>
                {
                    m.Property(x => x.Amount).IsRequired();
                    m.Property(x => x.Currency).IsRequired();
                });
                c.Navigation(x => x.RefundableCost).IsRequired();
            }).Navigation("_costAfterChange").IsRequired();

        builder.OwnsMany<PaymentDeadlineChange>("_paymentDeadlineChanges");

        builder.OwnsOne<PassengerNamesDeadlineChange>("_passengerNamesDeadlineChange");
    }
}