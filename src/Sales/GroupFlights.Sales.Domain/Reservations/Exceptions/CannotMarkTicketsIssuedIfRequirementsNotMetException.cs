using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Exceptions;

public class CannotMarkTicketsIssuedIfRequirementsNotMetException : HumanPresentableException
{
    public CannotMarkTicketsIssuedIfRequirementsNotMetException() : base(
        "Nie można oznaczyć wystawienia biletów jeśli nie zostały spełnione wszystkie wymagania!", ExceptionCategory.ConcurrencyError)
    {
    }
}