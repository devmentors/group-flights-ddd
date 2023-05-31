using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record PassengerNamesDeadlineRequestedEvent(
    Deadline Deadline, 
    Message Message,
    UnconfirmedReservation Reservation) : IDomainEvent;