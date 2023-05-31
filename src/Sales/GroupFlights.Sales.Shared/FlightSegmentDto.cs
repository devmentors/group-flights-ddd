using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Shared;

public record FlightSegmentDto(DateTime Date, Airport SourceAirport, Airport TargetAirport, FlightTimeDto FlightTime)
{
    private FlightSegmentDto() : this(default, default, default, default)
    {}
}