using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Changes.Payments;
using GroupFlights.Postsale.Domain.Changes.Request;
using GroupFlights.Postsale.Domain.Shared;
using GroupFlights.Shared.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Postsale.Infrastructure.Data.EF.Configs;

internal class ReservationChangeRequestConfig : IEntityTypeConfiguration<ReservationChangeRequest>
{
    public void Configure(EntityTypeBuilder<ReservationChangeRequest> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne<ReservationToChange>("_reservationToChange");
        builder.Property<DateTime>("_newTravelDate");
        
        builder.Property<UserId>("_requester")
            .HasConversion<Guid?>(x => x == null ? null : x.Value, 
                x => x == null ? null : new (x.Value))
            .IsRequired();

        builder.Property<bool>("_isFeasible");
        builder.OwnsOne<RequiredPayment>("_paymentRequiredToApplyChange", p =>
        {
            p.OwnsOne(x => x.Deadline);
        });
        
        builder.OwnsOne<ReservationCost>("_newCost",
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
                
            }).Navigation("_newCost").IsRequired(false);
        
        builder.OwnsMany<FlightSegment>("_newTravel", t =>
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

        builder.HasOne<ReservationChangeToApply>("_changeToApply");

        builder.Property<bool>("_isActive");
        builder.Property<ReservationChangeRequest.CompletionStatus?>("_completionStatus");
    }
}