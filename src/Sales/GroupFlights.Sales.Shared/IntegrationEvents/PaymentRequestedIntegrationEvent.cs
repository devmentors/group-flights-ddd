using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Sales.Shared.IntegrationEvents;

public record PaymentRequestedIntegrationEvent(Guid PaymentId,
    Guid PayerId,
    Money Amount,
    DateTime DueDate,
    PaymentRequestedSource Source) : IEvent;
public record PaymentRequestedSource(string SourceType, Guid SourceId);