using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Finance.Shared.Events;

public record PaymentCompleted(Guid PaymentId) : IEvent;