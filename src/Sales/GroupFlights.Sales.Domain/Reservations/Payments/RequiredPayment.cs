using GroupFlights.Sales.Domain.Shared;

namespace GroupFlights.Sales.Domain.Reservations.Payments;

public record RequiredPayment(Guid PaymentId, Deadline Deadline, bool Payed = false);