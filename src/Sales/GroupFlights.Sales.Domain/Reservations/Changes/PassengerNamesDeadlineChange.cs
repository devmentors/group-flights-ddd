namespace GroupFlights.Sales.Domain.Reservations.Changes;

public record PassengerNamesDeadlineChange(Guid DeadlineId, DateTime NewDueDate);