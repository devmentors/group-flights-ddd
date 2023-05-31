namespace GroupFlights.Sales.Shared;

public record DeadlineDto(DateTime DueDate, bool? Fullfilled, Guid? DeadlineId = default);