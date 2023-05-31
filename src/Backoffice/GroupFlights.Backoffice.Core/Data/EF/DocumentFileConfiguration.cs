using GroupFlights.Backoffice.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.Backoffice.Core.Data;

internal class DocumentFileConfiguration : IEntityTypeConfiguration<DocumentFile>
{
    public void Configure(EntityTypeBuilder<DocumentFile> builder)
    {
        builder.HasKey(x => x.FileId);

        builder.Property(x => x.Content).HasColumnType("bytea").IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.ContractId).IsRequired(false);
        
        builder.Property(x => x.Owner)
            .HasConversion(x => x.Value, x => new (x))
            .IsRequired();
        
        
    }
}