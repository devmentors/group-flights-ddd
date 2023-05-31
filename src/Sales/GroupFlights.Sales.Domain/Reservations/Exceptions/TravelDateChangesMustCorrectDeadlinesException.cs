using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class TravelDateChangesMustCorrectDeadlinesException : HumanPresentableException
{
    public TravelDateChangesMustCorrectDeadlinesException() 
        : base("Zmiana daty lotu na rezerwacji musi zawierać informacje o zmienionych deadline'ach", ExceptionCategory.ValidationError)
    {
    }
}