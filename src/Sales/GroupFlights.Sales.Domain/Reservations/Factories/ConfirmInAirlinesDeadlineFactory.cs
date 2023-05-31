using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Reservations.Factories;

public class ConfirmInAirlinesDeadlineFactory
{
    private readonly IReadOnlyCollection<UserId> _availableCashiers;

    public ConfirmInAirlinesDeadlineFactory(IReadOnlyCollection<UserId> availableCashiersUserIds)
    {
        _availableCashiers = availableCashiersUserIds ?? throw new ArgumentNullException(nameof(availableCashiersUserIds));
    }
    
    public ConfirmInAirlinesDeadlineRequestedEvent Create(Reservation reservation, OfferVariant variantChosenByClient)
    {
        var dueDate = variantChosenByClient.ValidTo.ValidToInAirlines;

        return new ConfirmInAirlinesDeadlineRequestedEvent(
            reservation,
            _availableCashiers, 
            new Deadline(new DeadlineId(Guid.NewGuid()), dueDate),
            ReservationToConfirmInAirlinesMessage(dueDate));
    }
    
    private static Message ReservationToConfirmInAirlinesMessage(DateTime dueDate) => new Message(
        $"Nowa rezerwacja oczekuje na potwierdzenie w liniach lotniczych! Deadline do: {dueDate.ToString("o")}."
    );
}