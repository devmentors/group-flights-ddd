using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.TimeManagement.Shared.Events;

public record DeadlineOverdueIntegrationEvent(DeadlineId Id) : IEvent;