using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Shared.Plumbing.Commands;

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}