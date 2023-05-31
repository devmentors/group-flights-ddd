using GroupFlights.Shared.Plumbing.Database;
using GroupFlights.WorkloadManagement.Core.Data;
using GroupFlights.WorkloadManagement.Core.Data.EF;
using GroupFlights.WorkloadManagement.Core.ModuleApi;
using GroupFlights.WorkloadManagement.Core.Services;
using GroupFlights.WorkloadManagement.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.WorkloadManagement.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWorkloadService, WorkloadService>();
        services.AddScoped<IWorkloadManagementApi, WorkloadManagementApi>();
        services.AddScoped<IWorkloadRepository, WorkloadRepository>();
        services.AddPostgres<WorkloadsDbContext>(configuration);
        return services;
    }
}