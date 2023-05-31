using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class VariantShouldConsistOfAtLeastOneFlightException : HumanPresentableException
{
    public VariantShouldConsistOfAtLeastOneFlightException(AirlineOfferId variantId) 
        : base($"Wariant oferty o numerze {variantId} nie posiada przynajmniej jednego przelotu", ExceptionCategory.ValidationError)
    {
    }
}