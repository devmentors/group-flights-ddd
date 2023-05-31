using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Postsale.Domain.Exceptions;

public class InvalidPaymentSetupException : HumanPresentableException
{
    public InvalidPaymentSetupException() 
        : base("Skonfigurowana płatność zawiera niepełne (lub nieprawidłowe) dane", ExceptionCategory.ValidationError)
    {
    }
}