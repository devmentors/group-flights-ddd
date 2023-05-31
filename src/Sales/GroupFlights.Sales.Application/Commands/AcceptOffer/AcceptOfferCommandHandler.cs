using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Plumbing.UserContext;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.Commands.AcceptOffer;

public class AcceptOfferCommandHandler : ICommandHandler<AcceptOfferCommand>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IClock _clock;
    private readonly IUserContextAccessor _userContextAccessor;

    public AcceptOfferCommandHandler(IOfferRepository offerRepository, 
        IEventDispatcher eventDispatcher, 
        IClock clock, 
        IUserContextAccessor userContextAccessor)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _userContextAccessor = userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));
    }
    
    public async Task HandleAsync(AcceptOfferCommand command, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.GetOfferById(command.OfferId, cancellationToken);
        var user = _userContextAccessor.Get();
        offer.AcceptVariant(command.VariantId, _clock, user?.UserId);

        await _offerRepository.UpdateOffer(offer, cancellationToken);
        await _eventDispatcher.PublishMultiple(offer.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }
}