using GroupFlights.Postsale.Application.Queries.GetChangeRequests;
using GroupFlights.Postsale.Infrastructure.Data.EF.Configs;
using GroupFlights.Shared.Plumbing.Database;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Postsale.Infrastructure.Data.EF;

public class PostsaleReadDbContext : DbContext, IDoNotMigrate
{
    public DbSet<ChangeRequestBasicData> ChangeRequests { get; set; }
    
    public PostsaleReadDbContext(DbContextOptions<PostsaleReadDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("postsale");
        modelBuilder.ApplyConfiguration(new ChangeRequestBasicDataConfig());
    }
}