using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Offers.Exceptions;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.DomainServices;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Shared.Plumbing.Events;

namespace GroupFlights.Sales.Application.ApplicationServices;

public class ReservationConfirmationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ReservationConfirmationDomainService _confirmationService;

    public ReservationConfirmationService(IOfferRepository offerRepository,
        IReservationRepository reservationRepository,
        IEventDispatcher eventDispatcher,
        ReservationConfirmationDomainService confirmationService)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
    }
    
    public async Task TryConfirmReservation(UnconfirmedReservation unconfirmedReservation, CancellationToken cancellationToken)
    {
        if (unconfirmedReservation.CanConfirmReservation())
        {
            var reservation = await _confirmationService.Confirm(unconfirmedReservation, cancellationToken);
            
            await _reservationRepository.ReplaceUnconfirmedWithConfirmedReservation(reservation, cancellationToken);

            await _eventDispatcher.PublishMultiple(reservation.DomainEvents
                .Select(@event => @event.RemapToPublicEvent())
                .SelectMany(e => e), cancellationToken);
        }
        else
        {
            await _reservationRepository.UpdateUnconfirmedReservation(unconfirmedReservation, cancellationToken);
        }
        
        await _eventDispatcher.PublishMultiple(unconfirmedReservation.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }
}