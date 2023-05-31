using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class ThisReservationDoesNotAllowPassengerChangesException : HumanPresentableException
{
    public ThisReservationDoesNotAllowPassengerChangesException() 
        : base("Ta rezerwacja nie pozwala na zmiany pasażerów po ich podaniu", ExceptionCategory.ConcurrencyError)
    {
    }
}