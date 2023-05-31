using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class PaymentSetupDoesNotExactlyCoverTotalCostException : HumanPresentableException
{
    public PaymentSetupDoesNotExactlyCoverTotalCostException() 
        : base("Ustawione płatności nie pokrywają dokładnie całości kosztu podróży", ExceptionCategory.ValidationError)
    {
    }
}