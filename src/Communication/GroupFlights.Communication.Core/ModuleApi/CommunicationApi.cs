using GroupFlights.Communication.Shared;
using GroupFlights.Communication.Shared.Models;
using GroupFlights.Shared.Types;
using Microsoft.Extensions.Logging;

namespace GroupFlights.Communication.Core.ModuleApi;

internal class CommunicationApi : ICommunicationApi
{
    private readonly ILogger<CommunicationApi> _logger;

    public CommunicationApi(ILogger<CommunicationApi> logger)
    {
        _logger = logger;
    }
    
    public Task SendMessageToRegisteredUser(UserId userId, CommunicationChannel channel, Message message,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            $@"Sent message: 
{message.Content}
to user: {userId}
through channel: {channel.ToString()}");
        
        return Task.CompletedTask;
    }

    public Task SendMessageToAnonymousUser(Email email, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            $@"Sent message: {message.Content}
to anonymous user's email: {email.Value}");

        return Task.CompletedTask;
    }
}