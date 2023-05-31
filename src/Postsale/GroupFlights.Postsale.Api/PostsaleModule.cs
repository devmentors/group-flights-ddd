using GroupFlights.Postsale.Application;
using GroupFlights.Postsale.Application.Commands.RequestReservationChange;
using GroupFlights.Postsale.Application.Commands.SetReservationChangeFeasibility;
using GroupFlights.Postsale.Application.Queries.GetChangeRequests;
using GroupFlights.Postsale.Infrastructure;
using GroupFlights.Shared.ModuleDefinition;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Postsale.Api;

public class PostsaleModule : ModuleDefinition
{
    public override string ModulePrefix => "/postsale";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication(configuration);
        services.AddInfrastructure(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        return new[]
        {
            new EndpointRegistration(
                "reservation-change-request", 
                HttpVerb.POST,
                NaiveAccessControl.ClientOnly,
                async (
                    [FromServices] ICommandHandler<RequestReservationChangeCommand> handler,
                    [FromBody] RequestReservationChangeCommand command,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(command, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status201Created);
                }),
            new EndpointRegistration(
                "reservation-change-request/feasibility", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] ICommandHandler<SetReservationChangeFeasibilityCommand> handler,
                    [FromBody] SetReservationChangeFeasibilityCommand command,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(command, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "reservation-change-request", 
                HttpVerb.GET,
                NaiveAccessControl.ClientAndCashier,
                async (
                    [FromServices] IQueryHandler<GetChangeRequestsQuery, ChangeRequestBasicData[]> handler,
                    [FromQuery] int pageNumber,
                    [FromQuery] int pageSize,
                    CancellationToken cancellationToken) =>
                {
                    return await handler.HandleAsync(
                        new GetChangeRequestsQuery
                        {
                            PageSize = pageSize,
                            PageNumber = pageNumber
                        }, 
                        cancellationToken);
                }),
        };
    }
}