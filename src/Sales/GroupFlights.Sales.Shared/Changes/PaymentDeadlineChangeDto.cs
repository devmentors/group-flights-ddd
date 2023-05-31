namespace GroupFlights.Sales.Shared.Changes;

public record PaymentDeadlineChangeDto(Guid PaymentId, DateTime NewDueDate, Guid DeadlineId);