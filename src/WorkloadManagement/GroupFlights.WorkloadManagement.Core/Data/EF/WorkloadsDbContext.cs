using GroupFlights.WorkloadManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.WorkloadManagement.Core.Data.EF;

internal class WorkloadsDbContext : DbContext
{
    public DbSet<CashierWorkloadAssignment> Workloads { get; set; }

    public WorkloadsDbContext(DbContextOptions<WorkloadsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("workloads");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}    
