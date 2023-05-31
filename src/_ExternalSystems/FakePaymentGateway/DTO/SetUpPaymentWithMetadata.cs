namespace FakePaymentGateway.DTO;

public record SetUpPaymentWithMetadata(
    Guid PaymentId,
    string PayerFullName,
    string TaxNumber,
    string Address,
    Money Amount,
    DateTime DueDate,
    string Secret,
    string WebhookUrl);