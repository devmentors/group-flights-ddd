using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Sales.Domain.Shared.External;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Reservations.Factories;

public class PassengerNamesDeadlineFactory
{
    private readonly IClock _clock;
    private readonly SalesConfiguration _salesConfiguration;
    
    public PassengerNamesDeadlineFactory(IClock clock, SalesConfiguration salesConfiguration)
    {
        _clock = clock;
        _salesConfiguration = salesConfiguration;
    }

    public PassengerNamesDeadlineRequestedEvent Create(UnconfirmedReservation unconfirmedReservation, OfferVariant variantChosenByClient)
    {
        var now = _clock.UtcNow;
        var deadlineToProvideNamesForFlight = TimeSpan.FromDays(14);

        var firstFlightCloserThanDeadline = variantChosenByClient.Travel.First().Date - now < deadlineToProvideNamesForFlight;
        var requiredImmediately =
            unconfirmedReservation.PassengerNamesRequiredImmediately || firstFlightCloserThanDeadline;
        
        var dueDate = requiredImmediately
            ? _clock.UtcNow.Add(_salesConfiguration.ContractSignTime)
            : variantChosenByClient.Travel.First().Date - deadlineToProvideNamesForFlight;

        var deadline = new Deadline(
            new DeadlineId(Guid.NewGuid()),
            dueDate);

        return new PassengerNamesDeadlineRequestedEvent(deadline, PassengerNamesMessage(requiredImmediately, dueDate),
            unconfirmedReservation);
    }

    private static Message PassengerNamesMessage(bool immediately, DateTime dueDate) => new Message(
        "Żeby przygotować twoje bilety, potrzebujemy podania nazwisk pasażerów. Musisz podać je " +
        (immediately ? "niezwłocznie" : $"do {dueDate.ToString("o")}")
    );
}