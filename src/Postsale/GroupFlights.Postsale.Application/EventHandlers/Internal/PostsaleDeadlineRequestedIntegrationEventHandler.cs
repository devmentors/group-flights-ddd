using GroupFlights.Communication.Shared.Models;
using GroupFlights.Postsale.Shared.IntegrationEvents;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.Postsale.Application.EventHandlers.Internal;

internal class PostsaleDeadlineRequestedIntegrationEventHandler : IEventHandler<PostsaleDeadlineRequestedIntegrationEvent>
{
    private readonly ITimeManagementApi _timeManagementApi;

    public PostsaleDeadlineRequestedIntegrationEventHandler(ITimeManagementApi timeManagementApi)
    {
        _timeManagementApi = timeManagementApi ?? throw new ArgumentNullException(nameof(timeManagementApi));
    }
    
    public async Task HandleAsync(PostsaleDeadlineRequestedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _timeManagementApi.SetUpDeadline(new SetUpDeadlineDto(
                new DeadlineId(@event.Id),
                CommunicationChannel.EmailAndNotification,
                @event.Participants.Select(p => new DeadlineParticipantDto(p.UserId, p.Email)).ToArray(),
                @event.Message,
                @event.DueDate),
            cancellationToken);
    }
}