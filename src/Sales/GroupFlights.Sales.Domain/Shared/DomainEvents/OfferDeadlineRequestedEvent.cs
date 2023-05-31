using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record OfferDeadlineRequestedEvent(Deadline Deadline, Message Message, Offer Offer) : IDomainEvent;