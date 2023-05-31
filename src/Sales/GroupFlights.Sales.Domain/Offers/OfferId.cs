namespace GroupFlights.Sales.Domain.Offers;

public record OfferId(Guid Value)
{
    public static implicit operator Guid(OfferId id) => id.Value;
    public static implicit operator OfferId(Guid id) => 
        id.Equals(Guid.Empty) ? null : new OfferId(id);
}