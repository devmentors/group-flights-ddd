using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class CannotAcceptVariantThatIsOverdueException : HumanPresentableException
{
    public CannotAcceptVariantThatIsOverdueException(AirlineOfferId variantId)
        : base($"Nie zaakceptowac oferty wariantu {variantId}, którego data akceptacji już minęła", 
            ExceptionCategory.ValidationError)
    {
    }
}