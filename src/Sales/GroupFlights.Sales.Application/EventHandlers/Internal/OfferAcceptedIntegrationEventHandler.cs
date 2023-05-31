using GroupFlights.Backoffice.Shared;
using GroupFlights.Sales.Application.EventMapping;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Factories;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Sales.Domain.Shared.External;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Time;
using GroupFlights.TimeManagement.Shared;
using DeadlineId = GroupFlights.TimeManagement.Shared.DeadlineId;

namespace GroupFlights.Sales.Application.EventHandlers.Internal;

internal class OfferAcceptedIntegrationEventHandler : IEventHandler<OfferAcceptedIntegrationEvent>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly ITimeManagementApi _timeManagementApi;
    private readonly IBackofficeApi _backofficeApi;
    private readonly IClock _clock;
    private readonly IEventDispatcher _eventDispatcher;

    public OfferAcceptedIntegrationEventHandler(IReservationRepository reservationRepository, 
        IOfferRepository offerRepository,
        ITimeManagementApi timeManagementApi,
        IBackofficeApi backofficeApi,
        IClock clock,
        IEventDispatcher eventDispatcher)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        _timeManagementApi = timeManagementApi ?? throw new ArgumentNullException(nameof(timeManagementApi));
        _backofficeApi = backofficeApi ?? throw new ArgumentNullException(nameof(backofficeApi));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task HandleAsync(OfferAcceptedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var offer = await _offerRepository.GetOfferById(new OfferId(@event.OfferId), cancellationToken);
        var salesConfig = await _backofficeApi.GetSalesConfiguration(cancellationToken);

        var salesConfiguration = new SalesConfiguration(salesConfig.DefaultOfferValidTime, salesConfig.ContractSignTime);
        var passengerNamesDeadlineFactory = new PassengerNamesDeadlineFactory(_clock, salesConfiguration);
        var contractGenerationRequestFactory = new ContractGenerationRequestFactory(_clock, salesConfiguration);
        
        var unconfirmedReservation = new UnconfirmedReservation(
            offer,
            new AirlineOfferId(@event.AcceptedVariantId),
            passengerNamesDeadlineFactory,
            contractGenerationRequestFactory);
        
        await _reservationRepository.AddUnconfirmedReservation(unconfirmedReservation, cancellationToken);
        
        await _eventDispatcher.PublishMultiple(unconfirmedReservation.DomainEvents
            .Select(@event => @event.RemapToPublicEvent())
            .SelectMany(e => e), cancellationToken);
    }
}