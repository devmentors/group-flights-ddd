using GroupFlights.Postsale.Application.EventMapping;
using GroupFlights.Postsale.Domain.Changes.Payments;
using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Postsale.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Application.Commands.SetReservationChangeFeasibility;

internal class SetReservationChangeFeasibilityCommandHandler : ICommandHandler<SetReservationChangeFeasibilityCommand>
{
    private readonly IReservationChangeRequestRepository _requestRepository;
    private readonly IEventDispatcher _eventDispatcher;

    public SetReservationChangeFeasibilityCommandHandler(IReservationChangeRequestRepository requestRepository,
        IEventDispatcher eventDispatcher)
    {
        _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(SetReservationChangeFeasibilityCommand command, CancellationToken cancellationToken = default)
    {
        var changeRequest = await _requestRepository.GetById(command.ChangeRequestId, cancellationToken);
        
        changeRequest.SetUpChangeFeasibility(
            command.IsFeasible, 
            new PaymentSetup(Guid.NewGuid(), 
                command.PaymentChangeRequiredToApplyChange.PayerId, 
                command.PaymentChangeRequiredToApplyChange.Cost,
                command.PaymentChangeRequiredToApplyChange.ChangeDeadline.DueDate),
            MapSegments(command.NewTravel));
        
        await _requestRepository.Update(changeRequest, cancellationToken);

        await _eventDispatcher.PublishMultiple(changeRequest.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }
    
    private static List<FlightSegment> MapSegments(IReadOnlyCollection<FlightSegmentDto> segments)
    {
        return segments.Select(_ => new FlightSegment(_.Date, _.SourceAirport, _.TargetAirport, new FlightTime(_.FlightTime.Hours, _.FlightTime.Minutes))).ToList();
    }
}