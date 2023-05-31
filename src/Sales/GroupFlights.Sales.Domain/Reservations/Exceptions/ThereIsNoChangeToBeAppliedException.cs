using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class ThereIsNoChangeToBeAppliedException : HumanPresentableException
{
    public ThereIsNoChangeToBeAppliedException() 
        : base("Żądanie zmiany rezerwacji nie zawiera żadnych zmian", ExceptionCategory.ValidationError)
    {
    }
}