namespace GroupFlights.Sales.Domain.Reservations;

public record ReservationId(Guid Value)
{
    public static implicit operator Guid(ReservationId id) => id.Value;
    public static implicit operator ReservationId(Guid id) => 
        id.Equals(Guid.Empty) ? null : new ReservationId(id);
}