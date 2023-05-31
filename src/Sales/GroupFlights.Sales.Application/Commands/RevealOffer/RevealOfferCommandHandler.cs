using GroupFlights.Communication.Shared;
using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Time;
using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.Sales.Application.Commands.RevealOffer;

internal class RevealOfferCommandHandler : ICommandHandler<RevealOfferCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IClock _clock;
    private readonly ICommunicationApi _communicationApi;
    private readonly IEventDispatcher _eventDispatcher;

    private static Message OfferRevealMessage(Offer offer) => new Message(
        $"Przygotowaliśmy dla Ciebię ofertę [{offer.OfferNumber}] - zaloguj się lub zarejestruj, by się z nią zapoznać");
    
    public RevealOfferCommandHandler(IOfferRepository offerRepository,
        IClock clock,
        ICommunicationApi communicationApi,
        IEventDispatcher eventDispatcher)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _communicationApi = communicationApi ?? throw new ArgumentNullException(nameof(communicationApi));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }

    public async Task HandleAsync(RevealOfferCommand command, CancellationToken cancellationToken = default)
    {
        var offerDraft = await _offerRepository.GetDraftById(command.OfferId, cancellationToken);
        var offer = offerDraft.RevealToClient(_clock);

        await _offerRepository.ReplaceDraftWithOffer(offer, cancellationToken);
        await _eventDispatcher.PublishMultiple(offer.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);

        await SendMessageResolved(cancellationToken, offer);
    }

    private async Task SendMessageResolved(CancellationToken cancellationToken, Offer offer)
    {
        if (offer.Client.UserId is not null)
        {
            await _communicationApi.SendMessageToRegisteredUser(
                offer.Client.UserId,
                CommunicationChannel.EmailAndNotification,
                OfferRevealMessage(offer),
                cancellationToken);
        }
        else
        {
            await _communicationApi.SendMessageToAnonymousUser(
                offer.Client.Email,
                OfferRevealMessage(offer),
                cancellationToken);
        }
    }
}