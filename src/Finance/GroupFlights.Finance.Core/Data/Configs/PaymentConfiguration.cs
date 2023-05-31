using GroupFlights.Finance.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Finance.Core.Data.Configs;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.PaymentId);
        builder.Property<Guid>(FinanceDbContext.VersionColumn).IsConcurrencyToken();
        
        builder.Property(x => x.PayerId).IsRequired();
        builder.OwnsOne(x => x.Amount);
        builder.Property(x => x.DueDate).IsRequired();
        builder.Property(x => x.PaymentGatewaySecret).IsRequired();
        builder.Property(x => x.Payed).IsRequired();
    }
}