using GroupFlights.Sales.Application.ApplicationServices;
using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Passengers;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Application.Commands.SetPassengersForFlightCommand;

public class SetPassengersForFlightCommandHandler : ICommandHandler<SetPassengersForFlightCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ReservationConfirmationService _reservationConfirmationService;
    private readonly IClock _clock;
    private readonly IEventDispatcher _eventDispatcher;

    public SetPassengersForFlightCommandHandler(IReservationRepository reservationRepository,
        ReservationConfirmationService reservationConfirmationService,
        IClock clock,
        IEventDispatcher eventDispatcher)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _reservationConfirmationService = reservationConfirmationService ?? throw new ArgumentNullException(nameof(reservationConfirmationService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(SetPassengersForFlightCommand command, CancellationToken cancellationToken = default)
    {
        UnconfirmedReservation unconfirmedReservation;
        try
        {
            unconfirmedReservation =
                await _reservationRepository.GetUnconfirmedReservationById(new ReservationId(command.ReservationId),
                    cancellationToken);
        }
        catch (DoesNotExistException ex)
        {
            unconfirmedReservation = null;
        }

        if (unconfirmedReservation is not null)
        {
            await SetPassengersForUnconfirmedReservation(command, unconfirmedReservation, cancellationToken);
        }
        else
        {
            await SetPassengersForReservation(command, cancellationToken);
        }
    }

    private async Task SetPassengersForUnconfirmedReservation(SetPassengersForFlightCommand command,
        UnconfirmedReservation unconfirmedReservation,
        CancellationToken cancellationToken)
    {
        unconfirmedReservation.SetPassengersForFlight(command.Passengers.Select(Map).ToList(), _clock);
        await _reservationConfirmationService.TryConfirmReservation(unconfirmedReservation, cancellationToken);
    }

    private async Task SetPassengersForReservation(SetPassengersForFlightCommand command,
        CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository
            .GetReservationById(new ReservationId(command.ReservationId), cancellationToken);
        
        reservation.SetPassengersForFlight(command.Passengers.Select(Map).ToList(), _clock);
        
        await _reservationRepository.UpdateReservation(reservation, cancellationToken);
        await _eventDispatcher.PublishMultiple(reservation.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }
    
    private Passenger Map(PassengerDto dto)
    {
        var document = new TravelDocument(
            dto.Document.Type,
            dto.Document.Number,
            dto.Document.Series,
            dto.Document.ExpirationDate);

        return new Passenger(dto.Firstname, dto.Middlename, dto.Surname, document);
    }
}