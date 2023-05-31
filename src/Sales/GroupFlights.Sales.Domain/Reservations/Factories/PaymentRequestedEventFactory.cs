using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Reservations.Payments;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Reservations.Factories;

internal class PaymentRequestedEventFactory
{
    private readonly IClock _clock;

    public PaymentRequestedEventFactory(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public ICollection<ReservationPaymentRequestedEvent> Create(Reservation reservation, PaymentSetup[] paymentsToSetup)
    {
        var events = new List<ReservationPaymentRequestedEvent>();
        
        foreach (var paymentSetup in paymentsToSetup)
        {
            var relatedDeadline = reservation.RequiredPayments.SingleOrDefault(p =>
                p.PaymentId.Equals(paymentSetup.PaymentId))?.Deadline;
            
            var paymentRequestedEvent = new ReservationPaymentRequestedEvent(
                paymentSetup.PaymentId,
                paymentSetup.PayerId,
                paymentSetup.Amount,
                paymentSetup.DueDate,
                relatedDeadline?.Id,
                PaymentRequiredMessage(paymentSetup.DueDate),
                reservation);
            
            events.Add(paymentRequestedEvent);
        }

        return events;
    }

    private static Message PaymentRequiredMessage(DateTime dueDate) => new Message(
        $"Żeby przygotować twoje bilety, potrzebujemy uiszczenia wymaganej płatności. " +
        $"Prosimy o uiszczenie jej w terminie do {dueDate.ToString("o")}.");
}