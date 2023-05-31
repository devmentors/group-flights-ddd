namespace GroupFlights.Postsale.Domain.Shared.Base;

public abstract class DomainEventsSource
{
    protected Queue<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents
    {
        get
        {
            var result = _domainEvents.ToList();
            return result;
        }
    }
}