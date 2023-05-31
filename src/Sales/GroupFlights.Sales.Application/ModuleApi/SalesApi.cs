using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Sales.Shared.OfferDraft.Commands;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Queries;

namespace GroupFlights.Sales.Application.ModuleApi;

internal class SalesApi : ISalesApi
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public SalesApi(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
    }
    
    public async Task CreateOfferDraft(SubmitOfferDraftCommand request, CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(request, cancellationToken);
    }

    public async Task ChangeReservation(ChangeReservationCommand request, CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(request, cancellationToken);
    }

    public async Task<ReservationToChangeDto> GetReservationForChange(GetReservationForChangeQuery request, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(request, cancellationToken);
    }
}