using System.Net.Http.Json;
using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Core.External;
using GroupFlights.Finance.Core.Models;
using GroupFlights.Finance.Shared.SetupPayment;
using Microsoft.Extensions.Configuration;

namespace GroupFlights.Finance.Api.ExternalServices;

internal class FakePaymentGatewayFacade : IPaymentGatewayFacade
{
    private readonly FakePaymentGatewayOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public FakePaymentGatewayFacade(FakePaymentGatewayOptions options, IHttpClientFactory httpClientFactory)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }
    
    public async Task SetUpPayment(SetupPaymentDto setupPayment, Payer payer, string paymentGatewaySecret,
        CancellationToken cancellationToken = default)
    {
        var paymentGatewayUrl = _options.PaymentGatewayUrl;
        var webhookToCall = _options.WebhookUrl;
        
        var httpClient = _httpClientFactory.CreateClient("FakePaymentGateway");
        
        var result =await httpClient.PostAsJsonAsync(paymentGatewayUrl,
            new SetUpPaymentWithMetadata(
                setupPayment.PaymentId,
                payer.PayerFullName,
                payer.TaxNumber,
                payer.Address,
                setupPayment.Amount,
                setupPayment.DueDate,
                paymentGatewaySecret,
                webhookToCall), 
            cancellationToken);

        result.EnsureSuccessStatusCode();
    }
}