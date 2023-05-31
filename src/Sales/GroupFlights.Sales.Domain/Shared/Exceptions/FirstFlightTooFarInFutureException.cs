using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Shared.Exceptions;

public class FirstFlightTooFarInFutureException : HumanPresentableException
{
    public FirstFlightTooFarInFutureException(int expectedMonthsInFutureMax, DateTime actual) 
        : base($"Przyjmujemy zapytania max {expectedMonthsInFutureMax} miesiecy w przyszlosc; podano date: {actual}", 
            ExceptionCategory.ValidationError)
    {
        
    }
}