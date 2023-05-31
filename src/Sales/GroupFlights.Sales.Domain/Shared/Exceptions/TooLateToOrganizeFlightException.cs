using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Shared.Exceptions;

public class TooLateToOrganizeFlightException : HumanPresentableException
{
    public TooLateToOrganizeFlightException(int expectedDaysBuffer, DateTime actual) 
        : base($"Przyjmujemy zapytania dot. podrozy zaczynajacych sie najwczesniej za {expectedDaysBuffer} dni w przyszlosci; " +
               $"podano date: {actual}", 
            ExceptionCategory.ValidationError)
    {
        
    }
}