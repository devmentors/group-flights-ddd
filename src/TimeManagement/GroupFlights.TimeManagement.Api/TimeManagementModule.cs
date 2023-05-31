using GroupFlights.Shared.ModuleDefinition;
using GroupFlights.TimeManagement.Core;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.TimeManagement.Api;

public class TimeManagementModule : ModuleDefinition
{
    public override string ModulePrefix => "/time-management";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        return Array.Empty<EndpointRegistration>();
    }
}