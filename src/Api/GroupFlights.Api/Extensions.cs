using GroupFlights.Api.ErrorHandling;
using GroupFlights.Api.UserContext;
using GroupFlights.Sales.Application.Commands.CrossCuttings;
using GroupFlights.Shared.ModuleDefinition;
using GroupFlights.Shared.Plumbing;
using GroupFlights.Shared.Plumbing.Database;
using GroupFlights.Shared.Plumbing.UserContext;

namespace GroupFlights.Api;

internal static class Extensions
{
    public static void AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModules(configuration);
        services.AddSharedFramework();
        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddTransient<NaiveAccessControlMiddleware>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextAccessor, NaiveUserContextAccessor>();
        services.EnforceWorkloadManagementRulesInSales();
        
        services.ConfigurePostgres();
    }
}