using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class OfferAlreadyCompletedException : HumanPresentableException
{
    public OfferAlreadyCompletedException(Offer offer) 
        : base($"Oferta o numerze [{offer.OfferNumber}] została już zamknięta i żadne zmiany nie są możliwe!", 
            ExceptionCategory.ValidationError)
    {
    }
}