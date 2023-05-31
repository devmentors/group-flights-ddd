using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class InvalidPaymentSetupException : HumanPresentableException
{
    public InvalidPaymentSetupException() 
        : base("Skonfigurowana płatność zawiera niepełne (lub nieprawidłowe) dane", ExceptionCategory.ValidationError)
    {
    }
}