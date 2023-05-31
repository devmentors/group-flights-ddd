using GroupFlights.Backoffice.Shared;
using GroupFlights.Sales.Domain.Offers.Exceptions;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Reservations.Exceptions;
using GroupFlights.Sales.Domain.Reservations.Factories;

namespace GroupFlights.Sales.Domain.Reservations.DomainServices;

public class ReservationConfirmationDomainService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IBackofficeApi _backofficeApi;

    public ReservationConfirmationDomainService(IBackofficeApi backofficeApi, IOfferRepository offerRepository)
    {
        _backofficeApi = backofficeApi ?? throw new ArgumentNullException(nameof(backofficeApi));
        _offerRepository = offerRepository;
    }
    
    public async Task<Reservation> Confirm(UnconfirmedReservation unconfirmedReservation,
        CancellationToken cancellationToken = default)
    {
        var sourceOffer = await _offerRepository.GetOfferById(unconfirmedReservation.SourceOfferId, cancellationToken);
        var variantChosenByClient =
            sourceOffer.Variants.SingleOrDefault(v => v.AirlineOfferId == unconfirmedReservation.AirlineOfferId);
            
        if (variantChosenByClient is null)
        {
            throw new OfferDoesNotContainGivenVariant(unconfirmedReservation.AirlineOfferId);
        }
        
        if (unconfirmedReservation.CanConfirmReservation() is false)
        {
            throw new RequirementsNotMetToConfirmReservationException();
        }
        
        var availableCashiers = await _backofficeApi.GetAvailableCashiersUserIds(cancellationToken);
        var confirmInAirlinesDeadlineFactory = new ConfirmInAirlinesDeadlineFactory(availableCashiers);
            
        return new Reservation(unconfirmedReservation, variantChosenByClient, confirmInAirlinesDeadlineFactory);
    }
}