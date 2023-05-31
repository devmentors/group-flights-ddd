namespace GroupFlights.Finance.Core.DTO;

public record PaymentWebhookDto(Guid PaymentId, string PaymentGatewaySecret);