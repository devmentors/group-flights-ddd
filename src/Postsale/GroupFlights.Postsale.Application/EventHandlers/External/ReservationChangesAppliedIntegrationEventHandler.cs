using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;

namespace GroupFlights.Postsale.Application.EventHandlers.External;

public class ReservationChangesAppliedIntegrationEventHandler : IEventHandler<ReservationChangesAppliedIntegrationEvent>
{
    private readonly IReservationChangeRequestRepository _repository;

    public ReservationChangesAppliedIntegrationEventHandler(IReservationChangeRequestRepository repository)
    {
        _repository = repository;
    }
    
    public async Task HandleAsync(ReservationChangesAppliedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var changeRequest = await _repository.GetById(@event.ReservationChangeRequestId, cancellationToken);
        
        changeRequest.OnChangeApplied();
        
        await _repository.Update(changeRequest, cancellationToken);
    }
}