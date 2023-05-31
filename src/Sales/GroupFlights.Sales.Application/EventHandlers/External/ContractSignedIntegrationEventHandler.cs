using GroupFlights.Backoffice.Shared.IntegrationEvents;
using GroupFlights.Sales.Application.ApplicationServices;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.EventHandlers.External;

public class ContractSignedIntegrationEventHandler : IEventHandler<ContractSignedIntegrationEvent>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ReservationConfirmationService _reservationConfirmationService;
    private readonly IClock _clock;

    public ContractSignedIntegrationEventHandler(IReservationRepository reservationRepository,
        ReservationConfirmationService reservationConfirmationService,
        IClock clock)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _reservationConfirmationService = reservationConfirmationService ?? throw new ArgumentNullException(nameof(reservationConfirmationService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public async Task HandleAsync(ContractSignedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var unconfirmedReservation =
            await _reservationRepository.GetUnconfirmedReservationByContractId(@event.ContractId, cancellationToken);
        
        unconfirmedReservation.OnContractSigned(@event.ContractId, _clock);
        await _reservationConfirmationService.TryConfirmReservation(unconfirmedReservation, cancellationToken);
    }
}