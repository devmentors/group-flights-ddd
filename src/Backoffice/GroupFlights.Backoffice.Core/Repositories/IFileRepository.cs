using GroupFlights.Backoffice.Core.Models;

namespace GroupFlights.Backoffice.Core.Repositories;

internal interface IFileRepository
{
    Task<DocumentFile> GetFileByFileId(Guid fileId, CancellationToken cancellationToken);
    Task<DocumentFile> GetFileByContractId(Guid contractId, CancellationToken cancellationToken);
    Task UploadFile(DocumentFile file, CancellationToken cancellationToken);

}