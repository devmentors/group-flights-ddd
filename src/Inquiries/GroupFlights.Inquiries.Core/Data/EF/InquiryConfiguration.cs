using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Shared.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Inquiries.Core.Data.EF;

internal class InquiryConfiguration : IEntityTypeConfiguration<Inquiry>
{
    public void Configure(EntityTypeBuilder<Inquiry> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new (x))
            .IsRequired();

        builder.OwnsOne(x => x.Inquirer, i =>
        {
            i.Property(x => x.Email)
                .HasConversion(x => x.Value, x => new (x))
                .IsRequired();

            i.Property(x => x.UserId)
                .HasConversion<Guid?>(x => x == null ? null : x.Value, 
                    x => x == null ? null : new (x.Value))
                .IsRequired(false);
        });
        
        builder.OwnsOne(x => x.Travel, t =>
        {
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
        });
        
        builder.OwnsOne(x => x.Return, r =>
        {
            r.OwnsOne(x => x.SourceAirport, a =>
            {
                a.Property(x => x.Code)
                    .HasConversion(x => x.Value,
                        x => new(x))
                    .IsRequired();
            });
            r.OwnsOne(x => x.TargetAirport, a =>
            {
                a.Property(x => x.Code)
                    .HasConversion(x => x.Value,
                        x => new(x))
                    .IsRequired();
            });
        });
        
        builder.OwnsOne(x => x.DeclaredPassengers, dp =>
        {
            dp.Property(x => x.AdultCount);
            dp.Property(x => x.ChildrenCount);
            dp.Property(x => x.InfantCount);
        });
        builder.OwnsMany(x => x.Priorities);
            
        builder.Property(x => x.CheckedBaggageRequired).IsRequired();
        builder.Property(x => x.AdditionalServicesRequired).IsRequired();
        builder.Property(x => x.Comments).IsRequired(false);

        builder.Property(x => x.VerificationResult).IsRequired(false);
        builder.Property(x => x.RejectionReason).IsRequired(false);
        builder.Property(x => x.OfferId).IsRequired(false);
    }
}