using GroupFlights.Postsale.Domain.Changes.DomainService;
using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.UserContext;

namespace GroupFlights.Postsale.Application.Commands.RequestReservationChange;

public class RequestReservationChangeCommandHandler : ICommandHandler<RequestReservationChangeCommand>
{
    private readonly IReservationChangeRequestRepository _requestRepository;
    private readonly ReservationChangeRequestDomainService _changeRequestDomainService;
    private readonly IUserContextAccessor _userContextAccessor;

    public RequestReservationChangeCommandHandler(IReservationChangeRequestRepository requestRepository, 
        ReservationChangeRequestDomainService changeRequestDomainService,
        IUserContextAccessor userContextAccessor)
    {
        _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        _changeRequestDomainService = changeRequestDomainService ?? throw new ArgumentNullException(nameof(changeRequestDomainService));
        _userContextAccessor = userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));
    }
    
    public async Task HandleAsync(RequestReservationChangeCommand command, CancellationToken cancellationToken = default)
    {
        var (reservationToChangeId, newTravelDate, changeRequestId) = command;

        var changeRequest = await _changeRequestDomainService.CreateChangeRequest(
            reservationToChangeId,
            newTravelDate,
            _userContextAccessor.Get().UserId,
            changeRequestId,
            cancellationToken);

        await _requestRepository.Add(changeRequest, cancellationToken);
    }
}