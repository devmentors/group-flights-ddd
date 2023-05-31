namespace FakePaymentGateway.DTO;

public record PaymentWebhookDto(Guid PaymentId, string PaymentGatewaySecret);