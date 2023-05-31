using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Postsale.Shared.IntegrationEvents;

public record PostsalePaymentRequestedIntegrationEvent(Guid PaymentId,
    Guid PayerId,
    Money Amount,
    DateTime DueDate) : IEvent;