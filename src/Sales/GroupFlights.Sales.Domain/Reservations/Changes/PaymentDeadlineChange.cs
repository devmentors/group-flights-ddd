namespace GroupFlights.Sales.Domain.Reservations.Changes;

public record PaymentDeadlineChange(Guid PaymentId, DateTime NewDueDate);