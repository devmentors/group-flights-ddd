using GroupFlights.Postsale.Application.Queries.GetChangeRequests;
using GroupFlights.Postsale.Application.Repositories;
using GroupFlights.Postsale.Infrastructure.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Postsale.Infrastructure.Repositories;

public class ChangeRequestsReadOnlyRepository : IChangeRequestsReadOnlyRepository
{
    private readonly PostsaleReadDbContext _readDbContext;

    public ChangeRequestsReadOnlyRepository(PostsaleReadDbContext readDbContext)
    {
        _readDbContext = readDbContext ?? throw new ArgumentNullException(nameof(readDbContext));
    }
    
    public async Task<ChangeRequestBasicData[]> Browse(Guid requesterId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
    {
        return await _readDbContext.ChangeRequests
            .Where(x => x.RequesterId.Equals(requesterId))
            .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .OrderBy(x => x.Id)
            .ToArrayAsync(cancellationToken);
    }
}