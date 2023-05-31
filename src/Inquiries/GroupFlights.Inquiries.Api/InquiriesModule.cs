using GroupFlights.Inquiries.Core;
using GroupFlights.Inquiries.Core.DTO;
using GroupFlights.Inquiries.Core.External;
using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Inquiries.Core.Services;
using GroupFlights.Shared.ModuleDefinition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Inquiries.Api;

public class InquiriesModule : ModuleDefinition
{
    public override string ModulePrefix => "/inquiries";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        return new []
        {
            new EndpointRegistration(
                "airports", 
                HttpVerb.GET,
                NaiveAccessControl.Anonymous,
                async (
                    [FromServices] IIataAirportService airportsService,
                    [FromQuery] string queryPhrase,
                    CancellationToken cancellationToken) =>
                {
                    return await airportsService.GetAirportsByQueryPhrase(queryPhrase, cancellationToken);
                }),
            new EndpointRegistration(
                "", 
                HttpVerb.POST,
                NaiveAccessControl.Anonymous,
                async (
                    [FromServices] IInquiryService inquiryService,
                    [FromBody] InquiryInputDto inquiryDto,
                    CancellationToken cancellationToken) =>
                {
                    await inquiryService.SubmitInquiry(inquiryDto, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status201Created);
                }),
            new EndpointRegistration(
                "rejected/{inquiryId}", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] IInquiryService inquiryService,
                    [FromRoute] Guid inquiryId,
                    [FromBody] string reason,
                    CancellationToken cancellationToken) =>
                {
                    await inquiryService.RejectInquiry(new InquiryId(inquiryId), reason, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "accepted/{inquiryId}", 
                HttpVerb.POST,
                NaiveAccessControl.CashierOnly,
                async (
                    [FromServices] IInquiryService inquiryService,
                    [FromRoute] Guid inquiryId,
                    [FromBody] Guid? offerToCreateId,
                    CancellationToken cancellationToken) =>
                {
                    await inquiryService.AcceptInquiry(new InquiryId(inquiryId), offerToCreateId, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status200OK);
                }),
            new EndpointRegistration(
                "{inquiryId}", 
                HttpVerb.GET,
                NaiveAccessControl.ClientAndCashier,
                async (
                    [FromServices] IInquiryService inquiryService,
                    [FromRoute] Guid inquiryId,
                    CancellationToken cancellationToken) =>
                {
                    return await inquiryService.GetInquiryById(new InquiryId(inquiryId), cancellationToken);
                }),
        } ;
    }
}