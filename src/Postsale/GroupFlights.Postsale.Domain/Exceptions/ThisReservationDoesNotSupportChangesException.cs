using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Postsale.Domain.Exceptions;

public class ThisReservationDoesNotSupportChangesException : HumanPresentableException
{
    public ThisReservationDoesNotSupportChangesException() 
        : base("Ta rezerwacja nie wspiera zmian danych pasazerow, daty czy destynacji przelotu", ExceptionCategory.ValidationError)
    {
    }
}