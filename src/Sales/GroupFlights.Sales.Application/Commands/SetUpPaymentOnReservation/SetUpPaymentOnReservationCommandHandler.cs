using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Payments;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.Commands.SetUpPaymentOnReservation;

public class SetUpPaymentOnReservationCommandHandler : ICommandHandler<SetUpPaymentOnReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IClock _clock;

    public SetUpPaymentOnReservationCommandHandler(IReservationRepository reservationRepository, IEventDispatcher eventDispatcher, IClock clock)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public async Task HandleAsync(SetUpPaymentOnReservationCommand command, CancellationToken cancellationToken = default)
    {
        var reservation = await _reservationRepository.GetReservationById(new ReservationId(command.ReservationId), cancellationToken);

        if (reservation is null)
        {
            throw new DoesNotExistException();
        }
        
        reservation.SetUpPayments(
            (command.PaymentsToSetup ?? Array.Empty<PaymentToSetupDto>()).Select(Map).ToArray(),
            _clock);

        await _reservationRepository.UpdateReservation(reservation, cancellationToken);
        
        await _eventDispatcher.PublishMultiple(reservation.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }

    private static PaymentSetup Map(PaymentToSetupDto dto)
    {
        return new PaymentSetup(
            dto.PaymentId,
            dto.PayerId,
            dto.Amount,
            dto.DueDate);
    }
}