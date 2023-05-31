using GroupFlights.Sales.Application.DeadlineRegistry;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.Exceptions;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Time;
using GroupFlights.TimeManagement.Shared.Events;

namespace GroupFlights.Sales.Application.EventHandlers.External;

internal class DeadlineOverdueIntegrationEventHandler : IEventHandler<DeadlineOverdueIntegrationEvent>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IDeadlineRegistry _deadlineRegistry;
    private readonly IClock _clock;

    public DeadlineOverdueIntegrationEventHandler(
        IOfferRepository offerRepository, 
        IReservationRepository reservationRepository, 
        IDeadlineRegistry deadlineRegistry,
        IClock clock)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _deadlineRegistry = deadlineRegistry ?? throw new ArgumentNullException(nameof(deadlineRegistry));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public async Task HandleAsync(DeadlineOverdueIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var deadlineMapping = await _deadlineRegistry.GetByDeadlineId(@event.Id.Value, cancellationToken);

        switch (deadlineMapping.SourceType)
        {
            case nameof(Offer):
            {
                await OnOfferOverdue(@event, deadlineMapping, cancellationToken);
                break;
            }
            case nameof(UnconfirmedReservation):
            {
                await OnReservationOverdue(@event, deadlineMapping, cancellationToken);
                break;
            }
        }
    }

    private async Task OnOfferOverdue(DeadlineOverdueIntegrationEvent @event, DeadlineRegistryEntry deadlineMapping,
        CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetOfferById(deadlineMapping.SourceId, cancellationToken);
        
        if (!offer.AcceptOfferDeadline.Id.Value.Equals(@event.Id.Value))
        {
            throw new DeadlineMismatchException();
        }
        
        offer.OnAcceptOfferDeadlineNotMet(_clock);
        await _offerRepository.UpdateOffer(offer, cancellationToken);
    }

    private async Task OnReservationOverdue(DeadlineOverdueIntegrationEvent @event,
        DeadlineRegistryEntry deadlineMapping, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetReservationById(
            new ReservationId(deadlineMapping.SourceId), cancellationToken);

        if (reservation is not null)
        {
            var passengerNamesDeadlineMatched = reservation.PassengerNamesDeadline.Id.Value.Equals(@event.Id.Value);
            if (passengerNamesDeadlineMatched)
            {
                reservation.OnPassengerNamesDeadlineNotMet(_clock);
                await _reservationRepository.UpdateReservation(reservation, cancellationToken);
                return;
            }

            var matchingPayment = reservation.RequiredPayments
                .SingleOrDefault(p => p.Deadline.Id.Value.Equals(@event.Id.Value));

            if (matchingPayment is not null)
            {
                reservation.OnPaymentDeadlineNotMet(_clock, matchingPayment.PaymentId);
                await _reservationRepository.UpdateReservation(reservation, cancellationToken);
            }
            return;
        }

        var unconfirmedReservation = await _reservationRepository.GetUnconfirmedReservationById(
            new ReservationId(deadlineMapping.SourceId), cancellationToken);

        if (unconfirmedReservation.PassengerNamesDeadline.Id.Value.Equals(@event.Id.Value))
        {
            unconfirmedReservation.OnPassengerNamesDeadlineNotMet(_clock);
            await _reservationRepository.UpdateUnconfirmedReservation(unconfirmedReservation, cancellationToken);
            return;
        }

        if (unconfirmedReservation.ContractToSignDeadline.Id.Value.Equals(@event.Id.Value))
        {
            unconfirmedReservation.OnContractDeadlineNotMet(_clock);
            await _reservationRepository.UpdateUnconfirmedReservation(unconfirmedReservation, cancellationToken);
            return;
        }

        throw new DeadlineMismatchException();
    }
}