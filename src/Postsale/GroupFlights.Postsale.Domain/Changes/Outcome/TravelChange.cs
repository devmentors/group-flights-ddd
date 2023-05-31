using GroupFlights.Postsale.Domain.Shared;

namespace GroupFlights.Postsale.Domain.Changes.Outcome;

public record TravelChange(IReadOnlyCollection<FlightSegment> NewTravelSegments)
{
    private TravelChange() : this(new List<FlightSegment>())
    {
    }
}