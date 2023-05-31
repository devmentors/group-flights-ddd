using GroupFlights.Shared.Types.Events;

namespace GroupFlights.Backoffice.Shared.IntegrationEvents;

public record ContractSignedIntegrationEvent(Guid ContractId) : IEvent;