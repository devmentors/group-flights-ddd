using GroupFlights.Shared.Types;
using GroupFlights.WorkloadManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroupFlights.WorkloadManagement.Core.Data.EF;

internal class CashierWorkloadAssignmentConfig : IEntityTypeConfiguration<CashierWorkloadAssignment>
{
    public void Configure(EntityTypeBuilder<CashierWorkloadAssignment> builder)
    {
        builder.HasKey(x => x.AssignmentId);
        builder.Property(x => x.WorkloadType).IsRequired();
        builder.Property(x => x.WorkloadSourceId).IsRequired();
        builder.Property(x => x.CashierId)
            .HasConversion(x => x.Value, x => new CashierId(x))
            .IsRequired();
        
        builder.HasIndex(p => new { p.WorkloadType, p.WorkloadSourceId }).IsUnique();
        builder.HasIndex(p => new { p.CashierId, p.WorkloadType, p.WorkloadSourceId }).IsUnique();
    }
}