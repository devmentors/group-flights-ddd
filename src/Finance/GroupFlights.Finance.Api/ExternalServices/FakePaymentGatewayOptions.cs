namespace GroupFlights.Finance.Api.ExternalServices;

public record FakePaymentGatewayOptions(string PaymentGatewayUrl, string WebhookUrl);