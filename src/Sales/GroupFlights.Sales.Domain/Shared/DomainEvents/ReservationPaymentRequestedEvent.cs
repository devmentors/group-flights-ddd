using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;
using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record ReservationPaymentRequestedEvent(
    Guid PaymentId,
    Guid PayerId,
    Money Amount,
    DateTime DueDate,
    DeadlineId RelatedDeadlineId,
    Message NotificationMessage,
    Reservation Reservation) : IDomainEvent;