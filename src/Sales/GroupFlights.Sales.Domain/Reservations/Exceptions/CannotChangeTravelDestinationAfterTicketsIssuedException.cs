using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class CannotChangeTravelDestinationAfterTicketsIssuedException : HumanPresentableException
{
    public CannotChangeTravelDestinationAfterTicketsIssuedException() 
        : base("Zmiana trasy po wystawieniu biletów jest niemożliwa", ExceptionCategory.ValidationError)
    {
    }
}