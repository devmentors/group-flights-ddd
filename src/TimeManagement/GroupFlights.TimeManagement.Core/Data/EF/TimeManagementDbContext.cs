using GroupFlights.TimeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.TimeManagement.Core.Data.EF;

internal class TimeManagementDbContext : DbContext
{
    public DbSet<Deadline> Deadlines { get; set; }
    public DbSet<DeadlineNotification> Notifications { get; set; }

    public TimeManagementDbContext(DbContextOptions<TimeManagementDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("time-management");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}