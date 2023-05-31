using GroupFlights.Backoffice.Shared;
using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.External;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.Commands.AddOfferVariant;

internal class AddOfferVariantCommandHandler : ICommandHandler<AddOfferVariantCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IBackofficeApi _backofficeApi;
    private readonly IClock _clock;

    public AddOfferVariantCommandHandler(IOfferRepository offerRepository, IBackofficeApi backofficeApi, IClock clock)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _backofficeApi = backofficeApi ?? throw new ArgumentNullException(nameof(backofficeApi));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public async Task HandleAsync(AddOfferVariantCommand command, CancellationToken cancellationToken = default)
    {
        var (draftId, airlineType, airlineName, travel, 
            @return, airlineOfferId, validToFromAirlines) = command;
        var offerDraft = await _offerRepository.GetDraftById(new OfferId(draftId), cancellationToken);

        var salesConfig = await _backofficeApi.GetSalesConfiguration(cancellationToken);
        var cashierBuffer = await _backofficeApi.GetCashierBufferConfiguration(cancellationToken);
        
        offerDraft.AddNewVariant(
            airlineType: airlineType,
            airlineName: airlineName,
            travel: MapSegments(travel),
            @return: MapSegments(@return),
            variantId: new AirlineOfferId(airlineOfferId),
            salesConfiguration: new SalesConfiguration(salesConfig.DefaultOfferValidTime, salesConfig.ContractSignTime),
            cashierBuffer: new CashierBufferConfiguration(cashierBuffer.MinimalBufferInHours),
            clock: _clock,
            validToFromAirlines: validToFromAirlines);

        await _offerRepository.UpdateDraft(offerDraft, cancellationToken);
    }

    private static List<FlightSegment> MapSegments(IReadOnlyCollection<FlightSegmentDto> segments)
    {
        return segments.Select(_ => new FlightSegment(_.Date, _.SourceAirport, _.TargetAirport, new FlightTime(_.FlightTime.Hours, _.FlightTime.Minutes))).ToList();
    }
}