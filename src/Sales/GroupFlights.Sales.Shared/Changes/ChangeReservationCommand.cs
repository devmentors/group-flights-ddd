using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Shared.Changes;

public record ChangeReservationCommand(Guid ReservationId,
    Guid ReservationChangeRequestId,
    ChangeTravelDto ChangeTravel,
    NewTotalCostDto CostAfterChange,
    List<PaymentDeadlineChangeDto> PaymentDeadlineChanges,
    PassengerNamesDeadlineChangeDto PassengerNamesDeadlineChange) : ICommand;