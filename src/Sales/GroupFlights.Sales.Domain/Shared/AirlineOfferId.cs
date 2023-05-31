namespace GroupFlights.Sales.Domain.Shared;

public record AirlineOfferId(string Value)
{
    public static implicit operator string(AirlineOfferId code) => code?.Value;
    public static implicit operator AirlineOfferId(string code) => 
        string.IsNullOrEmpty(code) ? null : new AirlineOfferId(code);
}