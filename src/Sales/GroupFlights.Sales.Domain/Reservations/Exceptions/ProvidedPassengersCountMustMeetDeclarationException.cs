using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class ProvidedPassengersCountMustMeetDeclarationException : HumanPresentableException
{
    public ProvidedPassengersCountMustMeetDeclarationException() 
        : base("Liczba podanych pasażerów musi być równa deklaracji!", ExceptionCategory.ValidationError)
    {
    }
}