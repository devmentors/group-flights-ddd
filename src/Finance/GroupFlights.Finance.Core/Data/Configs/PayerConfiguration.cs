using GroupFlights.Finance.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Finance.Core.Data.Configs;

public class PayerConfiguration : IEntityTypeConfiguration<Payer>
{
    public void Configure(EntityTypeBuilder<Payer> builder)
    {
        builder.HasKey(x => x.PayerId);
        builder.Property<Guid>(FinanceDbContext.VersionColumn).IsConcurrencyToken();
        builder.Property(x => x.PayerFullName).IsRequired();
        builder.Property(x => x.IsLegalEntity).IsRequired();
        builder.Property(x => x.TaxNumber);
        builder.Property(x => x.Address).IsRequired();
    }
}