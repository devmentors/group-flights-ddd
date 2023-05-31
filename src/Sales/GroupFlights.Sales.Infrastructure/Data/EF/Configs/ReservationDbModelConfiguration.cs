using GroupFlights.Sales.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Sales.Infrastructure.Data.EF.Configs;

internal class ReservationDbModelConfiguration : IEntityTypeConfiguration<ReservationDbModel>
{
    public void Configure(EntityTypeBuilder<ReservationDbModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Version).IsConcurrencyToken();
        builder.Property(x => x.Object).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.ContractId).IsRequired(false);
    }
}