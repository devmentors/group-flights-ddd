using GroupFlights.Postsale.Application.Repositories;
using GroupFlights.Shared.Plumbing.Queries;
using GroupFlights.Shared.Plumbing.UserContext;

namespace GroupFlights.Postsale.Application.Queries.GetChangeRequests;

public class GetChangeRequestQueryHandler : IQueryHandler<GetChangeRequestsQuery, ChangeRequestBasicData[]>
{
    private readonly IChangeRequestsReadOnlyRepository _readRepository;
    private readonly IUserContextAccessor _userContextAccessor;

    public GetChangeRequestQueryHandler(IChangeRequestsReadOnlyRepository readRepository, IUserContextAccessor userContextAccessor)
    {
        _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        _userContextAccessor = userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));
    }
    
    public async Task<ChangeRequestBasicData[]> HandleAsync(GetChangeRequestsQuery query, CancellationToken cancellationToken = default)
    {
        var userId = _userContextAccessor.Get().UserId.Value;
        return await _readRepository.Browse(userId, query.PageSize, query.PageNumber, cancellationToken);
    }
}