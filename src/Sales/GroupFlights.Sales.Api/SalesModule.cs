using GroupFlights.Sales.Application;
using GroupFlights.Sales.Application.Commands.AcceptOffer;
using GroupFlights.Sales.Application.Commands.AddOfferVariant;
using GroupFlights.Sales.Application.Commands.ConfirmVariant;
using GroupFlights.Sales.Application.Commands.MarkTicketsIssued;
using GroupFlights.Sales.Application.Commands.RevealOffer;
using GroupFlights.Sales.Application.Commands.SetPassengersForFlightCommand;
using GroupFlights.Sales.Application.Commands.SetUpPaymentOnReservation;
using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Application.Queries.Offers;
using GroupFlights.Sales.Application.Queries.Reservations;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Infrastructure;
using GroupFlights.Shared.ModuleDefinition;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Sales.Api;

public class SalesModule : ModuleDefinition
{
    public override string ModulePrefix => "/sales";
    
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
                "offers/{offerId}/variants", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] ICommandHandler<AddOfferVariantCommand> handler,
                    [FromRoute] Guid offerId,
                    [FromBody] AddOfferVariantCommand command,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(command, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status201Created);
                }),
            new EndpointRegistration(
                "offers/", 
                HttpVerb.GET,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] IQueryHandler<GetOffersQuery, List<OfferDto>> handler,
                    CancellationToken cancellationToken) =>
                {
                    return await handler.HandleAsync(new(), cancellationToken);
                }),
            new EndpointRegistration(
                "offers/{offerId}/variants/{airlineOfferId}/confirmation", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] ICommandHandler<ConfirmOfferVariantCommand> handler,
                    [FromRoute] Guid offerId,
                    [FromRoute] string airlineOfferId,
                    [FromBody] ConfirmOfferVariantCommand command,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(command, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "offers/{offerId}/revealed", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] ICommandHandler<RevealOfferCommand> handler,
                    [FromRoute] Guid offerId,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(new RevealOfferCommand(new OfferId(offerId)), cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "offers/{offerId}/variants/{airlineOfferId}/accepted", 
                HttpVerb.POST,
                NaiveAccessControl.ClientOnly,
                async (
                    [FromServices] ICommandHandler<AcceptOfferCommand> handler,
                    [FromRoute] Guid offerId,
                    [FromRoute] string airlineOfferId,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(new AcceptOfferCommand(
                            new OfferId(offerId),
                            new AirlineOfferId(airlineOfferId)),
                        cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "reservations", 
                HttpVerb.GET,
                NaiveAccessControl.ClientAndCashier,
                async (
                    [FromServices] IQueryHandler<GetReservationsQuery, List<ReservationDto>> handler,
                    CancellationToken cancellationToken) =>
                {
                    return await handler.HandleAsync(new GetReservationsQuery(), cancellationToken);
                }),
            new EndpointRegistration(
                "reservations/{reservationId}/passengers", 
                HttpVerb.POST,
                NaiveAccessControl.ClientOnly,
                async (
                    [FromServices] ICommandHandler<SetPassengersForFlightCommand> handler,
                    [FromRoute] Guid reservationId,
                    [FromBody] SetPassengersForFlightCommand command,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(command, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "reservations/{reservationId}/payments", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] ICommandHandler<SetUpPaymentOnReservationCommand> handler,
                    [FromRoute] Guid reservationId,
                    [FromBody] SetUpPaymentOnReservationCommand command,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(command, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "reservations/{reservationId}/completed", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] ICommandHandler<MarkTicketsIssuedCommand> handler,
                    [FromRoute] Guid reservationId,
                    [FromBody] MarkTicketsIssuedCommand command,
                    CancellationToken cancellationToken) =>
                {
                    await handler.HandleAsync(command, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
        };
    }
}