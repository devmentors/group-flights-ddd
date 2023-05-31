using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class CannotAddAlreadyOverdueVariant : HumanPresentableException
{
    public CannotAddAlreadyOverdueVariant() 
        : base("Nie można dodać do oferty wariantu, którego data akceptacji dla klienta już minęła", 
            ExceptionCategory.ValidationError)
    {
    }
}