using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Shared.Base;

namespace GroupFlights.Postsale.Domain.Changes.Events;

public record ReservationChangeAccepted(Guid ReservationChangeRequestId,
    Guid ReservationId,
    TravelChange TravelTravelChange,
    ReservationCost CostAfterChange,
    IReadOnlyCollection<PaymentDeadlineChange> PaymentDeadlineChanges,
    PassengerNamesDeadlineChange PassengerNamesDeadlineChange, ReservationChangeToApply ReservationChangeToApply) : IDomainEvent;