using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Postsale.Shared.IntegrationEvents;

public record ReservationChangeAcceptedIntegrationEvent(Guid ReservationId,
        Guid ReservationChangeRequestId,
        ChangeTravelDto ChangeTravel,
        NewTotalCostDto CostAfterChange,
        List<PaymentDeadlineChangeDto> PaymentDeadlineChanges,
        PassengerNamesDeadlineChangeDto PassengerNamesDeadlineChange) : IEvent;