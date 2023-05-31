using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class CannotOfferSameVariantTwiceException : HumanPresentableException
{
    public CannotOfferSameVariantTwiceException(AirlineOfferId variantId) 
        : base($"Nie mozna zaoferowac wiecej niz raz danego wariantu ({variantId.Value}), w ramach oferty", 
            ExceptionCategory.ValidationError)
    {
    }
}