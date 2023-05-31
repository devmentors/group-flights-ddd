using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Shared.Plumbing.Database;

public static class Extensions
{
    public static void ConfigurePostgres(this IServiceCollection services)
    {
        // Temporary fix for EF Core issue related to https://github.com/npgsql/efcore.pg/issues/2000
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public static void EnsureDatabaseCreated(this IServiceProvider serviceProvider)
    {
        var dbContexts = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
            .Where(type => typeof(DbContext).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .Where(type => type != typeof(DbContext))
            .ToList();

        using var scope = serviceProvider.CreateScope();
        
        var dbContextThatShouldMigrate = 
            dbContexts.Where(db => typeof(IDoNotMigrate).IsAssignableFrom(db) is false);
        
        foreach (var context in dbContextThatShouldMigrate)
        {
            (scope.ServiceProvider.GetService(context) as DbContext)?.Database.Migrate();   
        }
    }
    
    public static IServiceCollection AddPostgres<T>(this IServiceCollection services, IConfiguration configuration) where T : DbContext
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        services.AddDbContext<T>(x => x.UseNpgsql(connectionString));
    
        return services;
    }
}