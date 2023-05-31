using GroupFlights.Backoffice.Shared;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.External;
using GroupFlights.Shared.Plumbing.Commands;

namespace GroupFlights.Sales.Application.Commands.ConfirmVariant;

internal class ConfirmOfferVariantCommandHandler : ICommandHandler<ConfirmOfferVariantCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IBackofficeApi _backofficeApi;

    public ConfirmOfferVariantCommandHandler(IOfferRepository offerRepository, IBackofficeApi backofficeApi)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _backofficeApi = backofficeApi ?? throw new ArgumentNullException(nameof(backofficeApi));
    }
    
    public async Task HandleAsync(ConfirmOfferVariantCommand command, CancellationToken cancellationToken = default)
    {
        var (offerId, airlineOfferId, airportCharges, netPrice, confirmationLink) = command;

        var offerDraft = await _offerRepository.GetDraftById(new OfferId(offerId), cancellationToken);

        var feeConfig = await _backofficeApi.GetFeeConfiguration(cancellationToken);
        
        offerDraft.ConfirmVariant(
            new AirlineOfferId(airlineOfferId),
            airportCharges,
            netPrice,
            confirmationLink,
            new FeeConfiguration(new TicketFees(
                feeConfig.TicketFees.FeePerTicketOnShortHaulFlight, 
                feeConfig.TicketFees.FeePerTicketOnLongHaulFlight)));

        await _offerRepository.UpdateDraft(offerDraft, cancellationToken);
    }
}