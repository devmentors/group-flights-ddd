using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Sales.Domain.Shared.External;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Reservations.Factories;

public class ContractGenerationRequestFactory
{
    private readonly SalesConfiguration _salesConfiguration;
    private readonly IClock _clock;

    public ContractGenerationRequestFactory(IClock clock, SalesConfiguration salesConfiguration)
    {
        _salesConfiguration = salesConfiguration ?? throw new ArgumentNullException(nameof(salesConfiguration));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    public ContractGenerationRequestedEvent Create(UnconfirmedReservation unconfirmedReservation, OfferVariant variantChosenByClient)
    {
        var dueDate = _clock.UtcNow.Add(_salesConfiguration.ContractSignTime);
        
        var deadline = new Deadline(
            new DeadlineId(Guid.NewGuid()),
            dueDate);
        
        return new ContractGenerationRequestedEvent(
            unconfirmedReservation.ContractToSign.ContractId, 
            unconfirmedReservation,
            variantChosenByClient,
            deadline,
            ContractToSignMessage(dueDate));
    }
    
    private static Message ContractToSignMessage(DateTime dueDate) => new Message(
        $"Żeby przygotować twoje bilety, potrzebujemy podpisanej umowy. " +
        $"Prosimy o podpisanie jej i odesłanie niezwłocznie (masz czas do {dueDate.ToString("o")})."
    );
}