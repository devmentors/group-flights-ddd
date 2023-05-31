using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Application.Commands.MarkTicketsIssued;

public record MarkTicketsIssuedCommand(Guid ReservationId) : ICommand;