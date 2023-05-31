using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.WorkloadManagement.Core.Exceptions;

internal class WorkloadAlreadyAssignedToOtherCashierException : HumanPresentableException
{
    public WorkloadAlreadyAssignedToOtherCashierException() 
        : base("Inny kasjer już pracuje nad tym zadaniem!", ExceptionCategory.ValidationError)
    {
    }
}