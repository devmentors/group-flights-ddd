using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Changes;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.Commands.ChangeReservation;

internal class ChangeReservationCommandHandler : ICommandHandler<ChangeReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IClock _clock;
    private readonly IEventDispatcher _eventDispatcher;

    public ChangeReservationCommandHandler(IReservationRepository reservationRepository, 
        IClock clock, IEventDispatcher eventDispatcher)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(ChangeReservationCommand command, CancellationToken cancellationToken = default)
    {
        var reservation = await _reservationRepository.GetReservationById(
            new ReservationId(command.ReservationId), cancellationToken);
        
        reservation.ApplyReservationChange(MapReservationChange(command), _clock);

        await _reservationRepository.UpdateReservation(reservation, cancellationToken);

        await _eventDispatcher.PublishMultiple(reservation.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }

    private static ReservationChange MapReservationChange(ChangeReservationCommand command)
    {
        return new ReservationChange(
            command.ReservationChangeRequestId,
            new TravelChange(MapSegments(command.ChangeTravel.NewTravelSegments)
            ),
            new NewTotalCost(
                command.CostAfterChange.TotalCost,
                command.CostAfterChange.RefundableCost),
            command.PaymentDeadlineChanges.Select(p => 
                new PaymentDeadlineChange(p.PaymentId, p.NewDueDate)).ToList(),
            new PassengerNamesDeadlineChange(
                command.PassengerNamesDeadlineChange.DeadlineId, 
                command.PassengerNamesDeadlineChange.NewDueDate));
    }

    private static IReadOnlyCollection<FlightSegment> MapSegments(IReadOnlyCollection<FlightSegmentDto> segments)
    {
        return segments.Select(_ => new FlightSegment(_.Date, _.SourceAirport, _.TargetAirport, new FlightTime(_.FlightTime.Hours, _.FlightTime.Minutes))).ToList();
    }
}