using GroupFlights.Postsale.Domain.Changes.Request;
using GroupFlights.Postsale.Domain.Shared.Base;

namespace GroupFlights.Postsale.Domain.Changes.Events;

public record ReservationChangeRequestFinalized(Guid ChangeRequestId, ReservationChangeRequest.CompletionStatus CompletionStatus) : IDomainEvent;