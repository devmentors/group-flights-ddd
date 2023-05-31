using GroupFlights.Sales.Application.NaturalKeys;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Offers.Shared;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared.OfferDraft.Commands;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.Commands.SubmitOfferDraft;

internal class SubmitOfferDraftCommandHandler : ICommandHandler<SubmitOfferDraftCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly INaturalKeyFactory<OfferDraft> _offerDraftKeyFactory;
    private readonly IClock _clock;

    public SubmitOfferDraftCommandHandler(IOfferRepository offerRepository, INaturalKeyFactory<OfferDraft> offerNumberFactory, IClock clock)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _offerDraftKeyFactory = offerNumberFactory ?? throw new ArgumentNullException(nameof(offerNumberFactory));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public async Task HandleAsync(SubmitOfferDraftCommand command, CancellationToken cancellationToken = default)
    {
        var offerSource = CreateOfferSource(command);
        var client = CreateClient(command);
        var requestedTravel = CreateRequestedTravel(command, _clock);
        var priorities = CreatePriorities(command);

        var offerNumber = _offerDraftKeyFactory.CreateNaturalKey();
        var draft = new OfferDraft(offerNumber, offerSource, client, requestedTravel, priorities, command.OfferId);

        await _offerRepository.AddDraft(draft, cancellationToken);
    }
    
    private static OfferSource CreateOfferSource(SubmitOfferDraftCommand request)
    {
        return new OfferSource(
            sourceName: request.Source.SourceName, 
            sourceId: request.Source.SourceId);
    }

    private static Client CreateClient(SubmitOfferDraftCommand request)
    {
        return new Client(
            name: request.Client.Name,
            surname: request.Client.Surname,
            email: new Email(request.Client.Email),
            phoneNumber: request.Client.PhoneNumber,
            userId: string.IsNullOrEmpty(request.Client.UserId) ? null : new UserId(Guid.Parse(request.Client.UserId)));
    }

    private static RequestedTravel CreateRequestedTravel(SubmitOfferDraftCommand request, IClock clock)
    {
        var declaredPassengers = new PassengersData(
            infantCount: request.RequestedTravel.Passengers.InfantCount,
            childrenCount: request.RequestedTravel.Passengers.ChildrenCount,
            adultCount: request.RequestedTravel.Passengers.AdultCount);
        
        return new RequestedTravel(
            travel: new RequestedTravel.Flight(
                date: request.RequestedTravel.Travel.Date,
                sourceAirport: request.RequestedTravel.Travel.SourceAirport,
                targetAirport: request.RequestedTravel.Travel.TargetAirport),
            @return: new RequestedTravel.Flight(
                date: request.RequestedTravel.Return.Date,
                sourceAirport: request.RequestedTravel.Return.SourceAirport,
                targetAirport: request.RequestedTravel.Return.TargetAirport),
            passengers: declaredPassengers,
            checkedBaggageRequired: request.RequestedTravel.CheckedBaggageRequired,
            additionalServicesRequired: request.RequestedTravel.AdditionalServicesRequired,
            comments: request.RequestedTravel.Comments,
            clock);
    }
    
    private static IReadOnlyCollection<PriorityChoice> CreatePriorities(SubmitOfferDraftCommand request)
    {
        return request.Priorities
            .Select(p => PriorityChoice.Parse(feature: p.Feature, priority: p.Priority))
            .ToList();
    }
}