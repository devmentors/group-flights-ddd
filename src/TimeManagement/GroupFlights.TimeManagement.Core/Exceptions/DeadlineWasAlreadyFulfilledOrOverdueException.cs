using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.TimeManagement.Core.Exceptions;

public class DeadlineWasAlreadyFulfilledOrOverdueException : HumanPresentableException
{
    public DeadlineWasAlreadyFulfilledOrOverdueException() 
        : base("Próbowano oznaczyć stan deadline'u, mimo iż był już oznaczony jako spełniony lub przeterminowany", 
            ExceptionCategory.ValidationError)
    {
    }
}