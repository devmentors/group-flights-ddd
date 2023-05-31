namespace GroupFlights.Shared.Types.Time;

public class UtcClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}