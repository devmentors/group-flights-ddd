using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.TimeManagement.Core.Exceptions;

public class DeadlineIsAlreadyOverdueException : HumanPresentableException
{
    public DeadlineIsAlreadyOverdueException() 
        : base("Próbowano utworzyć deadline, który jest już przeterminowany", ExceptionCategory.ValidationError)
    {
    }
}