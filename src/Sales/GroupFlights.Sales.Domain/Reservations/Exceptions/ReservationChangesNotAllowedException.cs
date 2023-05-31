using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class ReservationChangesNotAllowedException : HumanPresentableException
{
    public ReservationChangesNotAllowedException() 
        : base("Zmiana tej rezerwacji nie jest mozliwa", ExceptionCategory.ValidationError)
    {
    }
}