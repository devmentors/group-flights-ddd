using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.Commands.MarkTicketsIssued;

internal class MarkTicketsIssuedCommandHandler : ICommandHandler<MarkTicketsIssuedCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IClock _clock;
    private readonly IEventDispatcher _eventDispatcher;

    public MarkTicketsIssuedCommandHandler(IReservationRepository reservationRepository, IClock clock, IEventDispatcher eventDispatcher)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(MarkTicketsIssuedCommand command, CancellationToken cancellationToken = default)
    {
        var reservation =
            await _reservationRepository.GetReservationById(new ReservationId(command.ReservationId), cancellationToken);

        reservation.MarkTicketsIssued(_clock);

        await _reservationRepository.UpdateReservation(reservation, cancellationToken);
        
        await _eventDispatcher.PublishMultiple(reservation.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }
}