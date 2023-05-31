using GroupFlights.Postsale.Domain.Changes.Events;
using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Changes.Payments;
using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Postsale.Domain.Changes.Request;
using GroupFlights.Postsale.Infrastructure.Data.EF;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Postsale.Infrastructure.Repositories;

internal class ReservationChangeRequestRepository : IReservationChangeRequestRepository
{
    private readonly PostsaleDbContext _dbContext;

    public ReservationChangeRequestRepository(PostsaleDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<bool> ExistsActiveForGivenUser(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChangeRequests.AnyAsync(c =>
            EF.Property<UserId>(c, "_requester").Equals(userId)
            && EF.Property<bool>(c, "_isActive") == true,
            cancellationToken);
    }

    public async Task Add(ReservationChangeRequest reservationChangeRequest, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.ChangeRequests
                .AnyAsync(c => c.Id == reservationChangeRequest.Id, cancellationToken);

        if (exists)
        {
            throw new AlreadyExistsException();
        }
        
        _dbContext.ChangeRequests.Add(reservationChangeRequest);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ReservationChangeRequest> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChangeRequests
            .Include("_reservationToChange")
            .Include("_changeToApply")
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<ReservationChangeRequest> GetByPaymentId(Guid paymentId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChangeRequests
            .Include("_reservationToChange")
            .Include("_changeToApply")
            .SingleOrDefaultAsync(c => 
                    EF.Property<RequiredPayment>(c, "_paymentRequiredToApplyChange").PaymentId.Equals(paymentId), 
                cancellationToken);
    }

    public async Task<ReservationChangeRequest> GetByDeadlineId(Guid deadlineId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ChangeRequests
            .Include("_reservationToChange")
            .Include("_changeToApply")
            .SingleOrDefaultAsync(c => 
                    EF.Property<RequiredPayment>(c, "_paymentRequiredToApplyChange").Deadline.Id.Equals(deadlineId), 
                cancellationToken);
    }

    public async Task Update(ReservationChangeRequest reservationChangeRequest, CancellationToken cancellationToken = default)
    {
        _dbContext.ChangeRequests.Update(reservationChangeRequest);

        var changeAcceptEvent = reservationChangeRequest
            .DomainEvents.OfType<ReservationChangeAccepted>().SingleOrDefault();

        if (changeAcceptEvent is not null)
        {
            _dbContext.ChangesToApply.Add(changeAcceptEvent.ReservationChangeToApply);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}