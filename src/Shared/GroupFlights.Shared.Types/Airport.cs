namespace GroupFlights.Shared.Types;

public record Airport(IataAirportCode Code, string City, string Name, string Country)
{
    private Airport() : this(null, null, null, null)
    {
    }
}

public class IataAirportCode
{
    private const int IataAirportCodeLength = 3;
    
    public string Value { get; private set; }

    private IataAirportCode() { }
    
    public IataAirportCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value.Length != IataAirportCodeLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        
        Value = value;
    }

    public static implicit operator string(IataAirportCode code) => code?.Value;
    public static implicit operator IataAirportCode(string code) => 
        string.IsNullOrEmpty(code) ? null : new IataAirportCode(code);
}