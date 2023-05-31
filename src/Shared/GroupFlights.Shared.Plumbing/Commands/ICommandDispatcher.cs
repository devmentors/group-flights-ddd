using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Shared.Plumbing.Commands;

public interface ICommandDispatcher
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : class, ICommand;
}