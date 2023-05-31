using System.Collections.ObjectModel;
using GroupFlights.Postsale.Application.DTO;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Postsale.Application.Commands.SetReservationChangeFeasibility;

public record SetReservationChangeFeasibilityCommand(Guid ChangeRequestId,
    bool IsFeasible,
    ChangeRequiredPaymentDto PaymentChangeRequiredToApplyChange,
    List<FlightSegmentDto> NewTravel) : ICommand;