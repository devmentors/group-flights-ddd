using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Sales.Shared.IntegrationEvents;

public record ReservationChangesAppliedIntegrationEvent(Guid ReservationChangeRequestId) : IEvent;