using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Sales.Shared.IntegrationEvents;

public record ContractGenerationRequestedIntegrationEvent(
    Guid ContractId,
    ContractSignee ContractSignee,
    ContractedTravel ContractedTravel,
    Money TotalCost) : IEvent;

public record ContractSignee(UserId UserId, string Name, string Surname);

public record ContractedTravel(string AirlineName, string AirlineOfferId, 
    IReadOnlyCollection<ContractedFlightSegment> Travel, IReadOnlyCollection<ContractedFlightSegment> Return);
public record ContractedFlightSegment(DateTime Date, Airport SourceAirport, Airport TargetAirport);
