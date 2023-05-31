using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class PaymentAlreadySetUpForReservationException : HumanPresentableException
{
    public PaymentAlreadySetUpForReservationException() 
        : base("Płatności na rezerwacji zostały już ustawione", ExceptionCategory.ValidationError)
    {
    }
}