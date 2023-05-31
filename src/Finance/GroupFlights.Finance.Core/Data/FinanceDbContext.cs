using GroupFlights.Finance.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Finance.Core.Data;

internal class FinanceDbContext : DbContext
{
    public const string VersionColumn = "Version";
    
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Payer> Payers { get; set; }
    
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
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
        modelBuilder.HasDefaultSchema("finance");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    
    private void UpdateVersions()
    {
        foreach (var change in ChangeTracker.Entries())
        {
            try
            {
                var versionProperty = change.Member(VersionColumn);
                versionProperty.CurrentValue = Guid.NewGuid();
            }
            catch
            {
            }
        }
    }
}