using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Application.DeadlineRegistry;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.Sales.Application.EventHandlers.Internal;

internal class DeadlineRequestedIntegrationEventHandler : IEventHandler<DeadlineRequestedIntegrationEvent>
{
    private readonly ITimeManagementApi _timeManagementApi;
    private readonly IDeadlineRegistry _deadlineRegistry;

    public DeadlineRequestedIntegrationEventHandler(ITimeManagementApi timeManagementApi, IDeadlineRegistry deadlineRegistry)
    {
        _timeManagementApi = timeManagementApi ?? throw new ArgumentNullException(nameof(timeManagementApi));
        _deadlineRegistry = deadlineRegistry ?? throw new ArgumentNullException(nameof(deadlineRegistry));
    }
    
    public async Task HandleAsync(DeadlineRequestedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _timeManagementApi.SetUpDeadline(new SetUpDeadlineDto(
                new DeadlineId(@event.Id),
                CommunicationChannel.EmailAndNotification,
                @event.Participants.Select(p => new DeadlineParticipantDto(p.UserId, p.Email)).ToArray(),
                @event.Message,
                @event.DueDate),
            cancellationToken);

        await _deadlineRegistry.SaveMapping(
            new DeadlineRegistryEntry(@event.Id, @event.Source.SourceType, @event.Source.SourceId), cancellationToken);
    }
}