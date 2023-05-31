using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;
using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record ConfirmInAirlinesDeadlineRequestedEvent(Reservation Reservation,
    IReadOnlyCollection<UserId> CashiersToNotify,
    Deadline Deadline,
    Message NotificationMessage) : IDomainEvent;