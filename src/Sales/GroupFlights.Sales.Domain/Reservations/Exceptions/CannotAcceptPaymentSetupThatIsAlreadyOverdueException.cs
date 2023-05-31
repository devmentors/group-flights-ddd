using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class CannotAcceptPaymentSetupThatIsAlreadyOverdueException : HumanPresentableException
{
    public CannotAcceptPaymentSetupThatIsAlreadyOverdueException() 
        : base("Nie można ustawić płatności, której data ważności już upłynęła", ExceptionCategory.ValidationError)
    {
    }
}