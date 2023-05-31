using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Sales.Shared.IntegrationEvents;

public record DeadlineMetIntegrationEvent(Guid DeadlineId) : IEvent;