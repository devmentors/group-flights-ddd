using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;

namespace GroupFlights.Sales.Domain.Shared.DomainEvents;

public record ContractGenerationRequestedEvent(Guid ContractId, 
    UnconfirmedReservation UnconfirmedReservation,
    OfferVariant VariantChosenByClient,
    Deadline Deadline,
    Message DeadlineReminderMessage) : IDomainEvent;