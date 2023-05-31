using GroupFlights.Sales.Domain.Reservations;

namespace GroupFlights.Sales.Infrastructure.Data.Models;

internal class ReservationDbModel : DbModel
{
    public static readonly string UnconfirmedReservationType = nameof(UnconfirmedReservation);
    public static readonly string ReservationType = nameof(Reservation);
    
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Object { get; set; }
    public Guid? ContractId { get; set; }
}