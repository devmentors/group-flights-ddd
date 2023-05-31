using GroupFlights.Finance.Shared.Events;
using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Application.PaymentRegistry;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Shared.Plumbing.Events;

namespace GroupFlights.Sales.Application.EventHandlers.External;

internal class PaymentCompletedEventHandler : IEventHandler<PaymentCompleted>
{
    private readonly IPaymentRegistry _paymentRegistry;
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventDispatcher _eventDispatcher;

    public PaymentCompletedEventHandler(IPaymentRegistry paymentRegistry, 
        IReservationRepository reservationRepository, 
        IEventDispatcher eventDispatcher)
    {
        _paymentRegistry = paymentRegistry ?? throw new ArgumentNullException(nameof(paymentRegistry));
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(PaymentCompleted @event, CancellationToken cancellationToken = default)
    {
        var paymentRegistryEntry = await _paymentRegistry.GetByPaymentId(@event.PaymentId, cancellationToken);

        if (paymentRegistryEntry is null)
        {
            return; //Payment z innego miejsca w systemie
        }

        switch (paymentRegistryEntry.SourceType)
        {
            case nameof(Reservation):
                await OnReservationPaymentPayed(paymentRegistryEntry, cancellationToken);
                break;
        }
        
    }

    private async Task OnReservationPaymentPayed(PaymentRegistryEntry paymentRegistryEntry, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository
            .GetReservationById(new ReservationId(paymentRegistryEntry.SourceId), cancellationToken);
        
        reservation.OnPaymentPayed(paymentRegistryEntry.PaymentId);
        
        await _reservationRepository.UpdateReservation(reservation, cancellationToken);
        
        await _eventDispatcher.PublishMultiple(reservation.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }
}