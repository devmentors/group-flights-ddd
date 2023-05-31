using GroupFlights.Inquiries.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Inquiries.Core.Data.EF;

internal class InquiriesDbContext : DbContext
{
    public DbSet<Inquiry> Inquiries { get; set; }

    public InquiriesDbContext(DbContextOptions<InquiriesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("inquiries");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}