using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Plumbing.Queries;
using GroupFlights.Shared.Types.Time;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Shared.Plumbing;

public static class Extensions
{
    public static IServiceCollection AddSharedFramework(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddEvents();
        services.AddQueries();
        services.AddTransient<IClock, UtcClock>();
        
        return services;
    }
}