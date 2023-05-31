using GroupFlights.Backoffice.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Backoffice.Core.Data;

internal class BackofficeDbContext : DbContext
{
    public const string VersionColumn = "Version";

    public DbSet<DocumentFile> Documents { get; set; }

    public BackofficeDbContext(DbContextOptions<BackofficeDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("backoffice");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}