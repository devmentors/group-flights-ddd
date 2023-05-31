using GroupFlights.Communication.Shared.Models;
using GroupFlights.Postsale.Domain.Changes.Events;
using GroupFlights.Postsale.Domain.Shared;
using GroupFlights.Postsale.Domain.Shared.Base;
using GroupFlights.Postsale.Shared.IntegrationEvents;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Shared.Types.Events;
using RequestedDeadlineParticipant = GroupFlights.Postsale.Shared.IntegrationEvents.RequestedDeadlineParticipant;

namespace GroupFlights.Postsale.Application.EventMapping;

internal static class DomainEventsRemapping
{
    public static ICollection<IEvent> RemapToPublicEvent(this IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            ReservationChangeFeasibilitySet @event => @event.Map(),
            ReservationChangeAccepted @event => new [] { @event.Map() },
            _ => throw new NotSupportedException($"{domainEvent.GetType().Name} cannot be remapped to integration (public) event!")
        };
    }
    
    private static ICollection<IEvent> Map(this ReservationChangeFeasibilitySet domainEvent)
    {
        var paymentToSetup = domainEvent.PaymentToSetup;
        
        var paymentRequested = new PostsalePaymentRequestedIntegrationEvent(
            paymentToSetup.PaymentId,
            paymentToSetup.PayerId,
            paymentToSetup.Amount,
            paymentToSetup.DueDate);

        var paymentDeadline = domainEvent.PaymentDeadline;
        
        var deadlineRequested = new PostsaleDeadlineRequestedIntegrationEvent(
            paymentDeadline.Id,
            paymentDeadline.DueDate,
            PaymentRequiredMessage(paymentDeadline.DueDate),
            new [] 
            { 
                new RequestedDeadlineParticipant(domainEvent.ChangeRequester, default) 
            });

        return new IEvent[] { paymentRequested, deadlineRequested };
    }

    private static IEvent Map(this ReservationChangeAccepted domainEvent)
    {
        return new ReservationChangeAcceptedIntegrationEvent(
            domainEvent.ReservationId,
            domainEvent.ReservationChangeRequestId,
            new ChangeTravelDto(domainEvent.TravelTravelChange.NewTravelSegments.Select(CreateFlightSegment).ToList()),
            new NewTotalCostDto(domainEvent.CostAfterChange.TotalCost, domainEvent.CostAfterChange.RefundableCost),
            domainEvent.PaymentDeadlineChanges.Select(p => 
                    new PaymentDeadlineChangeDto(p.PaymentId, p.NewDueDate, p.DeadlineId))
                .ToList(),
            new PassengerNamesDeadlineChangeDto(domainEvent.PassengerNamesDeadlineChange.DeadlineId,
                domainEvent.PassengerNamesDeadlineChange.NewDueDate));
    }
    
    private static FlightSegmentDto CreateFlightSegment(FlightSegment flightSegment)
    {
        return new FlightSegmentDto(flightSegment.Date,
            flightSegment.SourceAirport,
            flightSegment.TargetAirport,
            new FlightTimeDto(flightSegment.FlightTime.Hours, flightSegment.FlightTime.Minutes)
        );
    }
    
    private static Message PaymentRequiredMessage(DateTime dueDate) => new(
        $"Żeby zmienić twoją rezerwację potrzebujemy uiszczenia wymaganej płatności. " +
        $"Prosimy o uiszczenie jej w terminie do {dueDate.ToString("o")}.");
}