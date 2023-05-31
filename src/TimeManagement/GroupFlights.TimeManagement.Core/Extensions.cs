using GroupFlights.Shared.Plumbing.Database;
using GroupFlights.TimeManagement.Core.Data.EF;
using GroupFlights.TimeManagement.Core.ModuleApi;
using GroupFlights.TimeManagement.Core.Schedulers;
using GroupFlights.TimeManagement.Core.Services;
using GroupFlights.TimeManagement.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.TimeManagement.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITimeManagementApi, TimeManagementApi>();
        services.AddScoped<IDeadlineService, DeadlineService>();
        services.AddHostedService<DeadlineScheduler>();
        services.AddPostgres<TimeManagementDbContext>(configuration);

        return services;
    }
}