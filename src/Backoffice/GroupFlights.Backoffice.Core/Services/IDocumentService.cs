using GroupFlights.Backoffice.Core.DTO;
using GroupFlights.Sales.Shared.IntegrationEvents;

namespace GroupFlights.Backoffice.Core.Services;

public interface IDocumentService
{
    Task GenerateContract(ContractGenerationRequestedIntegrationEvent @event, CancellationToken cancellationToken);
    Task<DocumentFileDto> DownloadContractFile(Guid contractId, CancellationToken cancellationToken);
    Task UploadSignedContract(SubmitContractFileDto contractFileDto, CancellationToken cancellationToken);
}