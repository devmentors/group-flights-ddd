using GroupFlights.Backoffice.Core.Services;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;

namespace GroupFlights.Backoffice.Core.EventHandlers;

internal class ContractGenerationRequestedEventHandler : IEventHandler<ContractGenerationRequestedIntegrationEvent>
{
    private readonly IDocumentService _documentService;

    public ContractGenerationRequestedEventHandler(IDocumentService documentService)
    {
        _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
    }
    
    public async Task HandleAsync(ContractGenerationRequestedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _documentService.GenerateContract(@event, cancellationToken);
    }
}