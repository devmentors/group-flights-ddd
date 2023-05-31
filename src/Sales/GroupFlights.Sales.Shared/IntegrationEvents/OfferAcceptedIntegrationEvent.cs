using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Sales.Shared.IntegrationEvents;

public record OfferAcceptedIntegrationEvent(Guid OfferId, string AcceptedVariantId) : IEvent;