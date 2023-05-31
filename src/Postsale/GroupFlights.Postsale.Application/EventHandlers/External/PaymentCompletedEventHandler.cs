using GroupFlights.Finance.Shared.Events;
using GroupFlights.Postsale.Application.EventMapping;
using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Shared.Plumbing.Events;

namespace GroupFlights.Postsale.Application.EventHandlers.External;

internal class PaymentCompletedEventHandler : IEventHandler<PaymentCompleted>
{
    private readonly IReservationChangeRequestRepository _repository;
    private readonly IEventDispatcher _eventDispatcher;

    public PaymentCompletedEventHandler(IReservationChangeRequestRepository repository, 
        IEventDispatcher eventDispatcher)
    {
        _repository = repository;
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(PaymentCompleted @event, CancellationToken cancellationToken = default)
    {
        var changeRequest = await _repository.GetByPaymentId(@event.PaymentId, cancellationToken);
        
        if (changeRequest is null)
        {
            return; //Payment z innego miejsca w systemie
        }
        
        changeRequest.OnChangePayed();
        
        await _repository.Update(changeRequest, cancellationToken);
        
        await _eventDispatcher.PublishMultiple(changeRequest.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
        
    }
}