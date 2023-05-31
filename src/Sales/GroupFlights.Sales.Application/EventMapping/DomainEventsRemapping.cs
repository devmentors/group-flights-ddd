using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Sales.Application.EventMapping;

internal static class DomainEventsRemapping
{
    public static ICollection<IEvent> RemapToPublicEvent(this IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            OfferAcceptedEvent @event => @event.Map(), 
            OfferDeadlineRequestedEvent @event => new [] { @event.Map() },
            PassengerNamesDeadlineRequestedEvent @event => new[] { @event.Map() },
            ContractGenerationRequestedEvent @event => @event.Map(),
            ReservationRequirementMetEvent @event => new [] { @event.Map() },
            ConfirmInAirlinesDeadlineRequestedEvent @event => new [] { @event.Map() },
            ReservationPaymentRequestedEvent @event => @event.Map(),
            ReservationChangesAppliedEvent @event => new [] { @event.Map() },
            _ => throw new NotSupportedException($"{domainEvent.GetType().Name} cannot be remapped to integration (public) event!")
        };
    }
    
    public static ICollection<IEvent> Map(this OfferAcceptedEvent domainEvent)
    {
        return new IEvent[] 
        { 
            new OfferAcceptedIntegrationEvent(domainEvent.Offer.Id.Value, domainEvent.Variant.AirlineOfferId.Value),
            new DeadlineMetIntegrationEvent(domainEvent.Offer.AcceptOfferDeadline.Id.Value)
        };
    }

    public static IEvent Map(this OfferDeadlineRequestedEvent domainEvent)
    {
        return new DeadlineRequestedIntegrationEvent(
            domainEvent.Deadline.Id.Value, 
            domainEvent.Deadline.DueDate, 
            domainEvent.Message,
            new [] { new RequestedDeadlineParticipant(domainEvent.Offer.Client.UserId, domainEvent.Offer.Client.Email) },
            new RequestedDeadlineSource(nameof(Offer), domainEvent.Offer.Id));
    }

    public static IEvent Map(this PassengerNamesDeadlineRequestedEvent domainEvent)
    {
        return new DeadlineRequestedIntegrationEvent(
            domainEvent.Deadline.Id.Value, 
            domainEvent.Deadline.DueDate, 
            domainEvent.Message,
            new [] { new RequestedDeadlineParticipant(domainEvent.Reservation.Client.UserId, domainEvent.Reservation.Client.Email) },
            new RequestedDeadlineSource(nameof(UnconfirmedReservation), domainEvent.Reservation.Id.Value));
    }

    public static ICollection<IEvent> Map(this ContractGenerationRequestedEvent domainEvent)
    {
        var reservation = domainEvent.UnconfirmedReservation;
        var variantChosenByClient = domainEvent.VariantChosenByClient;
        
        var contractSignee = new ContractSignee(
            reservation.Client.UserId, 
            reservation.Client.Name,
            reservation.Client.Surname);

        var contractedTravel = new ContractedTravel(
            variantChosenByClient.AirlineName,
            variantChosenByClient.AirlineOfferId,
            variantChosenByClient.Travel.Select(s => new ContractedFlightSegment(s.Date, s.SourceAirport, s.TargetAirport))
                .ToList(),
            variantChosenByClient.Return.Select(s => new ContractedFlightSegment(s.Date, s.SourceAirport, s.TargetAirport))
                .ToList());


        var contractRequestedEvent = new ContractGenerationRequestedIntegrationEvent(
            domainEvent.ContractId,
            contractSignee,
            contractedTravel,
            variantChosenByClient.Cost.TotalCost);

        var deadlineRequestedEvent = new DeadlineRequestedIntegrationEvent(
            domainEvent.Deadline.Id.Value,
            domainEvent.Deadline.DueDate,
            domainEvent.DeadlineReminderMessage,
            new [] { new RequestedDeadlineParticipant(domainEvent.UnconfirmedReservation.Client.UserId,
                domainEvent.UnconfirmedReservation.Client.Email) },
            new RequestedDeadlineSource(nameof(UnconfirmedReservation), domainEvent.UnconfirmedReservation.Id.Value));

        return new IEvent[] { contractRequestedEvent, deadlineRequestedEvent };
    }

    public static IEvent Map(this ReservationRequirementMetEvent domainEvent)
    {
        return new DeadlineMetIntegrationEvent(domainEvent.DeadlineId.Value);
    }
    
    public static IEvent Map(this ConfirmInAirlinesDeadlineRequestedEvent domainEvent)
    {
        return new DeadlineRequestedIntegrationEvent(
            domainEvent.Deadline.Id.Value,
            domainEvent.Deadline.DueDate,
            domainEvent.NotificationMessage,
            domainEvent.CashiersToNotify
                .Select(cashierUserId => new RequestedDeadlineParticipant(cashierUserId, null))
                .ToArray(),
            new RequestedDeadlineSource(nameof(Reservation), domainEvent.Reservation.Id.Value));
    }

    public static IEvent[] Map(this ReservationPaymentRequestedEvent domainEvent)
    {
        var paymentRequested = new PaymentRequestedIntegrationEvent(
            domainEvent.PaymentId,
            domainEvent.PayerId,
            domainEvent.Amount,
            domainEvent.DueDate,
            new PaymentRequestedSource(nameof(Reservation), domainEvent.Reservation.Id.Value));

        var deadlineMatch = domainEvent.Reservation.RequiredPayments.Select(_ => _.Deadline)
            .SingleOrDefault(d => d.Id.Equals(domainEvent.RelatedDeadlineId));

        if (deadlineMatch is null)
        {
            throw new InvalidOperationException("Deadline not matched on event remapping!");
        }
        
        var deadlineRequested = new DeadlineRequestedIntegrationEvent(
            deadlineMatch.Id.Value,
            deadlineMatch.DueDate,
            domainEvent.NotificationMessage,
            new [] 
            { 
                new RequestedDeadlineParticipant(domainEvent.Reservation.Client.UserId,
                domainEvent.Reservation.Client.Email) 
            },
            new RequestedDeadlineSource(nameof(Reservation), domainEvent.Reservation.Id.Value));

        return new IEvent[] { paymentRequested, deadlineRequested };
    }

    public static IEvent Map(this ReservationChangesAppliedEvent domainEvent)
    {
        return new ReservationChangesAppliedIntegrationEvent(domainEvent.ReservationChangeRequestId);
    }
}