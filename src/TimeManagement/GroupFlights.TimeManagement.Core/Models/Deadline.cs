using GroupFlights.Communication.Shared.Models;
using GroupFlights.Shared.Types;
using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.TimeManagement.Core.Models;

internal class Deadline
{
    private Deadline(){}
    
    public Deadline(DeadlineId id,
        CommunicationChannel communicationChannel,
        DeadlineParticipant[] participants,
        Message message,
        DateTime deadlineDateUtc,
        IReadOnlyCollection<DeadlineNotification> notifications)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        CommunicationChannel = communicationChannel;
        Participants = participants ?? throw new ArgumentNullException(nameof(participants));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        DeadlineDateUtc = deadlineDateUtc;
        Notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
    }

    public DeadlineId Id { get; init; }
    public CommunicationChannel CommunicationChannel { get; init; }
    public IReadOnlyCollection<DeadlineParticipant> Participants { get; init; }
    public Message Message { get; init; }
    public DateTime DeadlineDateUtc { get; internal set; }
    public IReadOnlyCollection<DeadlineNotification> Notifications { get; init; }
    public bool? Fulfilled { get; set; }
}