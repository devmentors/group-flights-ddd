namespace GroupFlights.Shared.Types.Time;

public interface IClock
{
    DateTime UtcNow { get; }
}