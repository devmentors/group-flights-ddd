using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Postsale.Domain.Exceptions;

public class ThereIsNoChangeToBeAppliedException : HumanPresentableException
{
    public ThereIsNoChangeToBeAppliedException() 
        : base("Żądanie zmiany nie zawiera żadnych zmian", ExceptionCategory.ValidationError)
    {
    }
}