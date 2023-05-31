using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Shared.Plumbing.Events;

public interface IEventHandler<in TEvent> where TEvent : class, IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}