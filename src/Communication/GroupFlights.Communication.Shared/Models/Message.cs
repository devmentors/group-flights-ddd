namespace GroupFlights.Communication.Shared.Models;

public class Message
{
    private Message()
    {
    }

    public Message(string content, Guid? messageTemplateId = default)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
        MessageTemplateId = messageTemplateId;
    }
    
    public string Content { get; init; }
    public Guid? MessageTemplateId { get; init; }
}