using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.WorkloadManagement.Core.Services;

namespace GroupFlights.WorkloadManagement.Core.Exceptions;

public class WorkloadAssignmentLimitReached : HumanPresentableException
{
    public WorkloadAssignmentLimitReached(IWorkloadConfiguration workloadConfiguration) : 
        base("Przekroczono dopuszczalną liczbę przypisanych zadań. " +
             "Odepnij się od któregoś z obecnych zapytań, ofert czy rezerwacji by kontynuuować", ExceptionCategory.ValidationError)
    {
    }
}