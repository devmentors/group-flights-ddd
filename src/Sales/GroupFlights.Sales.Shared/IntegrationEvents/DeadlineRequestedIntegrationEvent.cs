using GroupFlights.Communication.Shared.Models;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Sales.Shared.IntegrationEvents;

public record DeadlineRequestedIntegrationEvent(
    Guid Id,
    DateTime DueDate,
    Message Message,
    RequestedDeadlineParticipant[] Participants,
    RequestedDeadlineSource Source) : IEvent;
public record RequestedDeadlineParticipant(UserId UserId, Email Email);
public record RequestedDeadlineSource(string SourceType, Guid SourceId);