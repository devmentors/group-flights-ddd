using GroupFlights.Communication.Shared.Models;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Postsale.Shared.IntegrationEvents;

public record PostsaleDeadlineRequestedIntegrationEvent(
    Guid Id,
    DateTime DueDate,
    Message Message,
    RequestedDeadlineParticipant[] Participants) : IEvent;
public record RequestedDeadlineParticipant(UserId UserId, Email Email);