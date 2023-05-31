using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Postsale.Domain.Exceptions;

public class ClientRequestedDifferentChangeDateThanProvidedException : HumanPresentableException
{
    public ClientRequestedDifferentChangeDateThanProvidedException() 
        : base("Wprowadzono inna date wylotu niz w prosbie o zmiane przeslanej przez klienta", ExceptionCategory.ValidationError)
    {
    }
}