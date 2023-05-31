using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Postsale.Domain.Exceptions;

public class ChangesArePossibleAfterFirstPaymentAppearsException : HumanPresentableException
{
    public ChangesArePossibleAfterFirstPaymentAppearsException() 
        : base("Zmiany beda mozliwe dopiero po pojawieniu sie pierwszej, skonfigurowanej przez nas oplacie", ExceptionCategory.ValidationError)
    {
    }
}