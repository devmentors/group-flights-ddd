using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Inquiries.Core.Exceptions;

public class MaxPassengersCountExceededException : HumanPresentableException
{
    public MaxPassengersCountExceededException(int expected, int actual) 
        : base($"Grupowe przeloty dotycza grupy max. {expected}; podano: {actual}", ExceptionCategory.ValidationError)
    {
        
    }
}