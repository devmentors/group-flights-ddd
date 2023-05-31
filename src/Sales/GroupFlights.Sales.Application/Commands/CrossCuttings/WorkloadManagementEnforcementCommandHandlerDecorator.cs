using GroupFlights.Sales.Application.Commands.AddOfferVariant;
using GroupFlights.Sales.Application.Commands.ConfirmVariant;
using GroupFlights.Sales.Application.Commands.RevealOffer;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.UserContext;
using GroupFlights.Shared.Types.Commands;
using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.WorkloadManagement.Shared;

namespace GroupFlights.Sales.Application.Commands.CrossCuttings;

internal class WorkloadManagementEnforcementCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly IWorkloadManagementApi _workloadManagementApi;
    private readonly IUserContextAccessor _userContextAccessor;

    public WorkloadManagementEnforcementCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, IWorkloadManagementApi workloadManagementApi, 
        IUserContextAccessor userContextAccessor)
    {
        _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        _workloadManagementApi = workloadManagementApi ?? throw new ArgumentNullException(nameof(workloadManagementApi));
        _userContextAccessor = userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        //We could check the appsettings.json settings here to enable/disable this feature on demand
        var enabled = false;

        if (enabled is false)
        {
            await _commandHandler.HandleAsync(command, cancellationToken);
            return;
        }
        
        var cashierId = _userContextAccessor.Get().CashierId;
        var check = command switch
        {
            AddOfferVariantCommand c => new WorkloadAccessCheck(nameof(Offer),
                c.OfferId.ToString(), cashierId),
            ConfirmOfferVariantCommand c => new WorkloadAccessCheck(nameof(Offer),
                c.OfferId.ToString(), cashierId),
            RevealOfferCommand c => new WorkloadAccessCheck(nameof(Offer),
                c.OfferId.ToString(), cashierId),
            _ => throw new InvalidOperationException("Cannot enforce workload management rule!")
        };

        var isAssigned = await _workloadManagementApi.CanAccessWorkload(check, cancellationToken);

        if (isAssigned)
        {
            await _commandHandler.HandleAsync(command, cancellationToken);
        }
        else
        {
            throw new NotAssignedToThisWorkloadException();
        }
    }
}