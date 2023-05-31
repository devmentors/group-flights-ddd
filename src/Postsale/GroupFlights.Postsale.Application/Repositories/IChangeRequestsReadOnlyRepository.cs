using GroupFlights.Postsale.Application.Queries.GetChangeRequests;

namespace GroupFlights.Postsale.Application.Repositories;

public interface IChangeRequestsReadOnlyRepository
{
    Task<ChangeRequestBasicData[]> Browse(Guid requesterId, int pageSize, int pageNumber, 
        CancellationToken cancellationToken = default);
}