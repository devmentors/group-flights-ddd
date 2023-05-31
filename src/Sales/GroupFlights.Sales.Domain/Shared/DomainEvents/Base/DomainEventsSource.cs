namespace GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

public abstract class DomainEventsSource
{
    protected Queue<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents
    {
        get
        {
            var result = _domainEvents.ToList();
            _domainEvents.Clear();
            return result;
        }
    }
}