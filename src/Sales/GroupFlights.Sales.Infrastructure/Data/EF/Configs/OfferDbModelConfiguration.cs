using GroupFlights.Sales.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Sales.Infrastructure.Data.EF.Configs;

internal class OfferDbModelConfiguration : IEntityTypeConfiguration<OfferDbModel>
{
    public void Configure(EntityTypeBuilder<OfferDbModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Version).IsConcurrencyToken();
        builder.Property(x => x.Object).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.Type).IsRequired();
    }
}