using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class OfferDoesNotContainGivenVariant : HumanPresentableException
{
    public OfferDoesNotContainGivenVariant(AirlineOfferId variantId) 
        : base($"Oferta nie zawiera wariantu od zadanym id z linii lotniczych: {variantId}", ExceptionCategory.NotFound)
    {
    }
}