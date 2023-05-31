using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Changes.Payments;
using GroupFlights.Postsale.Domain.Shared;
using GroupFlights.Postsale.Domain.Shared.Base;
using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Domain.Changes.Events;

public record ReservationChangeFeasibilitySet(
    Guid ReservationChangeRequestId,
    UserId ChangeRequester,
    bool IsFeasible,
    ReservationCost NewCost, 
    PaymentSetup PaymentToSetup, 
    Deadline PaymentDeadline, 
    IReadOnlyCollection<FlightSegment> NewTravel) : IDomainEvent;