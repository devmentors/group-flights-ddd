using GroupFlights.Sales.Application.DeadlineRegistry;
using GroupFlights.Sales.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Sales.Infrastructure.Data.EF;

internal class SalesDbContext : DbContext
{
    public DbSet<OfferDbModel> Offers { get; set; }
    public DbSet<ReservationDbModel> Reservations { get; set; }
    public DbSet<DeadlineRegistryEntry> DeadlineRegistryEntries { get; set; }
    
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
    {
    }

    public override int SaveChanges()
    {
        UpdateVersions();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateVersions();
        
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("sales");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    
    private void UpdateVersions()
    {
        foreach (var change in ChangeTracker.Entries())
        {
            try
            {
                var versionProp = change.Member(nameof(DbModel.Version));
                versionProp.CurrentValue = Guid.NewGuid();
                
                if (change.State == EntityState.Added)
                {
                    var createDateProp = change.Member(nameof(DbModel.CreateDateUtc));
                    createDateProp.CurrentValue = DateTime.UtcNow;   
                }
                else
                {
                    var updateDateProp = change.Member(nameof(DbModel.UpdateDateUtc));
                    updateDateProp.CurrentValue = DateTime.UtcNow;
                }
            } catch { }
        }
    }
}