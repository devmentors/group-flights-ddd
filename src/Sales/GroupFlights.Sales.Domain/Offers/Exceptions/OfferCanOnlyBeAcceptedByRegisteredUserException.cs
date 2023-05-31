using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class OfferCanOnlyBeAcceptedByRegisteredUserException : HumanPresentableException
{
    public OfferCanOnlyBeAcceptedByRegisteredUserException() 
        : base($"Oferte moze zaakceptowac wylacznie zarejestrowany uzytkownik!", ExceptionCategory.ValidationError)
    {
    }
}