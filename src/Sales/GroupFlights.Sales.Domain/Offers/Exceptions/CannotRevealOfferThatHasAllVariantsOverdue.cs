using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class CannotRevealOfferThatHasAllVariantsOverdue : HumanPresentableException
{
    public CannotRevealOfferThatHasAllVariantsOverdue() 
        : base("Nie można przedstawić oferty, która nie posiada żadnego ważnego wariantu", 
            ExceptionCategory.ValidationError)
    {
    }
}