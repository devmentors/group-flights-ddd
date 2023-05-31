using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Changes.Payments;
using GroupFlights.Postsale.Domain.Shared;
using GroupFlights.Sales.Shared;

namespace GroupFlights.Postsale.Domain.Changes.Request;

public record ReservationToChange(
    Guid ReservationId,
    AirlineType AirlineType,
    bool IsCompleted,
    ReservationCost CurrentCost,
    IReadOnlyCollection<FlightSegment> CurrentTravel,
    IReadOnlyCollection<RequiredPayment> CurrentPayments,
    Deadline PassengerNamesDeadline)
{
    private ReservationToChange() : this(default, default, default, default,
        default, default, default)
    {
        
    }
}