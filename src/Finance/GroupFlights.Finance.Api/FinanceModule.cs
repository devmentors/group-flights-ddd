using GroupFlights.Finance.Api.ExternalServices;
using GroupFlights.Finance.Core;
using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Core.External;
using GroupFlights.Finance.Core.Models;
using GroupFlights.Finance.Core.Services;
using GroupFlights.Shared.ModuleDefinition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Finance.Api;

public class FinanceModule : ModuleDefinition
{
    public override string ModulePrefix => "/finance";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
        services.AddSingleton(
            new FakePaymentGatewayOptions(
                configuration.GetValue<string>("PaymentIntegration:PaymentGatewayUrl"),
                configuration.GetValue<string>("PaymentIntegration:WebhookUrl")));
        
        services.AddScoped<IPaymentGatewayFacade, FakePaymentGatewayFacade>();
        services.AddHttpClient();
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        return new[]
        {
            new EndpointRegistration(
                "payers",
                HttpVerb.POST,
                NaiveAccessControl.ClientOnly,
                async (
                    [FromServices] IPayerService payerService,
                    [FromBody] Payer payerDto,
                    CancellationToken cancellationToken) =>
                {
                    await payerService.AddPayer(payerDto, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status201Created);
                }),
            new EndpointRegistration(
                "payment-webhook",
                HttpVerb.POST,
                NaiveAccessControl.Anonymous,
                async (
                    [FromServices] IPaymentService paymentService,
                    [FromBody] PaymentWebhookDto webhookDto,
                    CancellationToken cancellationToken) =>
                {
                    await paymentService.OnPaymentPayed(webhookDto, cancellationToken);
                    return Results.StatusCode(StatusCodes.Status201Created);
                })
        };
    }
}