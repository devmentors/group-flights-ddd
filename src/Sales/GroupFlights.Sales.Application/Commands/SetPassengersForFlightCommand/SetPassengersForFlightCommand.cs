using GroupFlights.Sales.Application.DTO;
using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Application.Commands.SetPassengersForFlightCommand;

public record SetPassengersForFlightCommand(Guid ReservationId, List<PassengerDto> Passengers) : ICommand;