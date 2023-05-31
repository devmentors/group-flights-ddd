using GroupFlights.Communication.Shared.Models;
using GroupFlights.Shared.Types;

namespace GroupFlights.Communication.Shared;

public interface ICommunicationApi
{
    Task SendMessageToRegisteredUser(UserId userId, CommunicationChannel channel, Message message, CancellationToken cancellationToken);
    Task SendMessageToAnonymousUser(Email email, Message message, CancellationToken cancellationToken);
}