using GroupFlights.Shared.Plumbing.Queries;

namespace GroupFlights.Postsale.Application.Queries.GetChangeRequests;

public class GetChangeRequestsQuery : IQuery<ChangeRequestBasicData[]>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}