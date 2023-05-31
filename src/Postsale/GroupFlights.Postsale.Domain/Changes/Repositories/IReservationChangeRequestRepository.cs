using GroupFlights.Postsale.Domain.Changes.Request;
using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Domain.Changes.Repositories;

public interface IReservationChangeRequestRepository
{
    Task<bool> ExistsActiveForGivenUser(UserId userId, CancellationToken cancellationToken = default);
    
    Task Add(ReservationChangeRequest reservationChangeRequest, CancellationToken cancellationToken = default);
    
    Task<ReservationChangeRequest> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<ReservationChangeRequest> GetByPaymentId(Guid paymentId, CancellationToken cancellationToken = default);
    Task<ReservationChangeRequest> GetByDeadlineId(Guid deadlineId, CancellationToken cancellationToken = default);

    Task Update(ReservationChangeRequest reservationChangeRequest, CancellationToken cancellationToken = default);
}