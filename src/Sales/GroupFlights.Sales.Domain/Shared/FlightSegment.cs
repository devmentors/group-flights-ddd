using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Shared;

public class FlightSegment
{
    private FlightSegment()
    {
        
    }
    
    public FlightSegment(DateTime date, Airport sourceAirport, Airport targetAirport, FlightTime flightTime)
    {
        Date = date;
        SourceAirport = sourceAirport ?? throw new ArgumentNullException(nameof(sourceAirport));
        TargetAirport = targetAirport ?? throw new ArgumentNullException(nameof(targetAirport));
        FlightTime = flightTime ?? throw new ArgumentNullException(nameof(flightTime));
    }

    public DateTime Date { get; init; }
    public Airport SourceAirport { get; init; }
    public Airport TargetAirport { get; init; }
    public FlightTime FlightTime { get; init; }
}