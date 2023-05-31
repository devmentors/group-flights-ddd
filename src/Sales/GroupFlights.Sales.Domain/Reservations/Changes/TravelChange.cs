using GroupFlights.Sales.Domain.Shared;

namespace GroupFlights.Sales.Domain.Reservations.Changes;

public record TravelChange(IReadOnlyCollection<FlightSegment> NewTravelSegments)
{
    private TravelChange() : this(new List<FlightSegment>())
    {
    }
}