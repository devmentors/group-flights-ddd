namespace GroupFlights.Communication.Shared.Models;

[Flags]
public enum CommunicationChannel
{
    None = 0,
    Email = 1,
    InternalNotification = 2,
    EmailAndNotification = Email | InternalNotification
}