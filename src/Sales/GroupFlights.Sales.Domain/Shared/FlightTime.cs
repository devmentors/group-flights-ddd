namespace GroupFlights.Sales.Domain.Shared;

public class FlightTime
{
    public ushort Hours { get; }
    public ushort Minutes { get; }

    public ushort TotalMinutes => (ushort)(Hours * 60 + Minutes);

    private FlightTime()
    {
        
    }
    
    public FlightTime(ushort hours, ushort minutes)
    {
        if (hours == 0 && minutes == 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        if (minutes > 60)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        Hours = hours;
        Minutes = minutes;
    }
}