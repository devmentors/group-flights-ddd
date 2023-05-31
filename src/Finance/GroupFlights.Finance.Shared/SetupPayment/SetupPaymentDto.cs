using GroupFlights.Shared.Types;

namespace GroupFlights.Finance.Shared.SetupPayment;

public record SetupPaymentDto(Guid PaymentId, Guid PayerId, Money Amount, DateTime DueDate);