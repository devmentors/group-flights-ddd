namespace GroupFlights.Sales.Domain.Shared;

public record Deadline(DeadlineId Id, DateTime DueDate, bool? Fulfilled = null);
public record DeadlineId(Guid Value);