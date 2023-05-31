using GroupFlights.Communication.Core;
using GroupFlights.Shared.ModuleDefinition;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Communication.Api;

public class CommunicationModule : ModuleDefinition
{
    public override string ModulePrefix => "/communication";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        return Array.Empty<EndpointRegistration>();
    }
}