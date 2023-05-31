using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Inquiries.Core.Exceptions;

public class ThisIsNotAGroupFlightException : HumanPresentableException
{
    public ThisIsNotAGroupFlightException(int expected, int actual) 
        : base($"Grupowe przeloty dotycza grupy min. {expected}; podano: {actual}", ExceptionCategory.ValidationError)
    {
    }
}