using GroupFlights.Shared.Types;

namespace GroupFlights.Inquiries.Core.Models;

internal record InquiredFlight(DateTime Date, Airport SourceAirport, Airport TargetAirport)
{
    public InquiredFlight() : this(default, default, default)
    {
    }
}