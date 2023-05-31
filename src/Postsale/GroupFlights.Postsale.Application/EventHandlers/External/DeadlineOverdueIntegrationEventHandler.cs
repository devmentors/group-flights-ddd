using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.TimeManagement.Shared.Events;

namespace GroupFlights.Postsale.Application.EventHandlers.External;

internal class DeadlineOverdueIntegrationEventHandler : IEventHandler<DeadlineOverdueIntegrationEvent>
{
    private readonly IReservationChangeRequestRepository _repository;
    private readonly IEventDispatcher _eventDispatcher;

    public DeadlineOverdueIntegrationEventHandler(IReservationChangeRequestRepository repository, 
        IEventDispatcher eventDispatcher)
    {
        _repository = repository;
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(DeadlineOverdueIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var changeRequest = await _repository.GetByDeadlineId(@event.Id.Value, cancellationToken);
        
        changeRequest.OnChangePayed();
        
        await _repository.Update(changeRequest, cancellationToken);
    }
}