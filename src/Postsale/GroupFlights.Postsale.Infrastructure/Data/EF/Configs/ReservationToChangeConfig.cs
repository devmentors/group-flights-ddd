using GroupFlights.Postsale.Domain.Changes.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Postsale.Infrastructure.Data.EF.Configs;

internal class ReservationToChangeConfig : IEntityTypeConfiguration<ReservationToChange>
{
    public void Configure(EntityTypeBuilder<ReservationToChange> builder)
    {
        builder.HasKey(x => x.ReservationId);
        builder.Property(x => x.AirlineType).IsRequired();
        builder.Property(x => x.IsCompleted).IsRequired();
        builder.OwnsOne(x => x.CurrentCost,
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
            }).Navigation(x => x.CurrentCost).IsRequired();

        builder.OwnsMany(x => x.CurrentTravel, t =>
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

        builder.OwnsMany(x => x.CurrentPayments, p =>
        {
            p.OwnsOne(x => x.Deadline);
        });

        builder.OwnsOne(x => x.PassengerNamesDeadline);
    }
}