using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class RequirementsNotMetToConfirmReservationException : HumanPresentableException 
{
    public RequirementsNotMetToConfirmReservationException() 
        : base("Nie wszystkie wymagania (pasażerowie lub podpisana umowa) zostały spełnione by potwierdzić rezerwację",
            ExceptionCategory.ValidationError)
    {
    }
}