using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Changes.Request;
using GroupFlights.Postsale.Infrastructure.Data.EF.Configs;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Postsale.Infrastructure.Data.EF;

internal class PostsaleDbContext : DbContext
{
    public DbSet<ReservationChangeRequest> ChangeRequests { get; set; }
    public DbSet<ReservationChangeToApply> ChangesToApply { get; set; }
    public DbSet<ReservationToChange> ReservationSnapshots { get; set; }
    
    public PostsaleDbContext(DbContextOptions<PostsaleDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("postsale");
        modelBuilder.ApplyConfiguration(new ReservationChangeRequestConfig());
        modelBuilder.ApplyConfiguration(new ReservationChangeToApplyConfig());
        modelBuilder.ApplyConfiguration(new ReservationToChangeConfig());
    }
}