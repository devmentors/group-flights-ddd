using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Shared.Plumbing.Events;

public interface IEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}