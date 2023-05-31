using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class VariantAlreadyConfirmedException : HumanPresentableException
{
    public VariantAlreadyConfirmedException(AirlineOfferId variantId) 
        : base($"Wariant oferty o numerze {variantId} zostal juz potwierdzony", ExceptionCategory.ValidationError)
    {
    }
}