namespace GroupFlights.Shared.Types;

public record CashierId(Guid Value)
{
    public static implicit operator Guid(CashierId id) => id.Value;
    public static implicit operator CashierId(Guid id) => 
        id.Equals(Guid.Empty) ? null : new CashierId(id);
}