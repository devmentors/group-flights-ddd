using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Postsale.Domain.Exceptions;

public class ChangeRequestIsNotActiveAnymoreException : HumanPresentableException
{
    public ChangeRequestIsNotActiveAnymoreException() 
        : base("Zadanie zmiany nie jest juz aktywne. Zloz kolejne, jesli chcesz naniesc kolejna zmiane na swoja rezerwacje", 
            ExceptionCategory.ValidationError)
    {
    }
}