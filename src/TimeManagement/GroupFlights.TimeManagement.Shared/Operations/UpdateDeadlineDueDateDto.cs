namespace GroupFlights.TimeManagement.Shared.Operations;

public record UpdateDeadlineDueDateDto(DeadlineId Id, DateTime NewDueDate);