using GroupFlights.Shared.ModuleDefinition;
using GroupFlights.WorkloadManagement.Core;
using GroupFlights.WorkloadManagement.Core.DTO;
using GroupFlights.WorkloadManagement.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.WorkloadManagement.Api;

public class WorkloadManagementModule : ModuleDefinition
{
    public override string ModulePrefix => "/workload-management";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
        services.AddSingleton<IWorkloadConfiguration, WorkloadConfiguration>(
            _ => new WorkloadConfiguration(10));
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        return new[]
        {
            new EndpointRegistration(
                "workload-assignment",
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] IWorkloadService workloadService,
                    [FromBody] AssignCashierToWorkload assignCashierDto,
                    CancellationToken cancellationToken) =>
                {
                    await workloadService.AssignCashierToWorkload(assignCashierDto, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "workload-assignment",
                HttpVerb.DELETE,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] IWorkloadService workloadService,
                    [FromBody] UnassignCashierFromWorkload assignCashierDto,
                    CancellationToken cancellationToken) =>
                {
                    await workloadService.UnassignCashierFromWorkload(assignCashierDto, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
        };
    }
}