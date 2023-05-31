using GroupFlights.Postsale.Domain.Changes.Request;

namespace GroupFlights.Postsale.Application.Queries.GetChangeRequests;

public class ChangeRequestBasicData
{
    public Guid Id { get; set; }
    public DateTime NewTravelDate { get; set; }
    public Guid ReservationId { get; set; }
    public Guid RequesterId { get; set; }
    public ReservationChangeRequest.CompletionStatus? CompletionStatus { get; set; }
    
}