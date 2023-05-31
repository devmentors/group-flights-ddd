using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record ReservationRequirementMetEvent(DeadlineId DeadlineId) : IDomainEvent;