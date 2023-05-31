using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record OfferAcceptedEvent(Offer Offer, OfferVariant Variant) : IDomainEvent;