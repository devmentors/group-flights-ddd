using GroupFlights.Communication.Shared.Models;
using GroupFlights.Shared.Types;

namespace GroupFlights.TimeManagement.Shared.Operations;

public record SetUpDeadlineDto(
    DeadlineId Id,
    CommunicationChannel CommunicationChannel,
    DeadlineParticipantDto[] Participants,
    Message Message,
    DateTime DeadlineDateUtc);
    
public record DeadlineParticipantDto(UserId UserId, Email Email);