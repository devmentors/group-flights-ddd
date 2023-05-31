namespace GroupFlights.Sales.Shared.Changes;

public record PassengerNamesDeadlineChangeDto(Guid DeadlineId, DateTime NewDueDate);