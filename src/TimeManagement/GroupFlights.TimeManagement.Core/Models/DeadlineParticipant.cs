using GroupFlights.Shared.Types;

namespace GroupFlights.TimeManagement.Core.Models;

public class DeadlineParticipant
{
    public DeadlineParticipant(Guid id, Guid? userId, Email email)
    {
        Id = id;
        UserId = userId;
        Email = email;
    }

    public Guid Id { get; init; }
    public Guid? UserId { get; init; }
    public Email Email { get; init; }
}