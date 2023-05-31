using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record ReservationChangesAppliedEvent(Guid ReservationChangeRequestId): IDomainEvent;