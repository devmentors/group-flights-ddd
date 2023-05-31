using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Postsale.Application;

internal static class PublishExtensions
{
    public static Task PublishMultiple(this IEventDispatcher eventDispatcher, IEnumerable<IEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        var publishTasks = domainEvents
            .Select(@event => eventDispatcher.PublishAsync(@event, cancellationToken));
        return Task.WhenAll(publishTasks);
    }
}