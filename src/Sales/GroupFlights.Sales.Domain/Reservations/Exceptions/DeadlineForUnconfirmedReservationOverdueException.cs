using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class DeadlineForUnconfirmedReservationOverdueException : HumanPresentableException
{
    public DeadlineForUnconfirmedReservationOverdueException() 
        : base("Deadline na dostarczenie informacji do finalizacji rezerwacji już upłynął", ExceptionCategory.ValidationError)
    {
    }
}