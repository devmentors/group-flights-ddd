using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record OfferRejectedEvent(OfferId OfferId, string Reason) : IDomainEvent;