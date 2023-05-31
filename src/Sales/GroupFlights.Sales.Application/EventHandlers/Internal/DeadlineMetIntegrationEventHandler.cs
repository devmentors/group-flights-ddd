using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.TimeManagement.Shared;

namespace GroupFlights.Sales.Application.EventHandlers.Internal;

internal class DeadlineMetIntegrationEventHandler : IEventHandler<DeadlineMetIntegrationEvent>
{
    private readonly ITimeManagementApi _timeManagementApi;

    public DeadlineMetIntegrationEventHandler(ITimeManagementApi timeManagementApi)
    {
        _timeManagementApi = timeManagementApi ?? throw new ArgumentNullException(nameof(timeManagementApi));
    }
    
    public async Task HandleAsync(DeadlineMetIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _timeManagementApi.MarkDeadlineFulfilled(new DeadlineId(@event.DeadlineId), cancellationToken);
    }
}