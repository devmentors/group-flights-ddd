using GroupFlights.Backoffice.Core.Data;
using GroupFlights.Backoffice.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Backoffice.Core.Repositories;

internal class FileRepository : IFileRepository
{
    private readonly BackofficeDbContext _dbContext;

    public FileRepository(BackofficeDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<DocumentFile> GetFileByFileId(Guid fileId, CancellationToken cancellationToken)
    {
        return await _dbContext.Documents.SingleOrDefaultAsync(x => x.FileId.Equals(fileId), cancellationToken);
    }

    public async Task<DocumentFile> GetFileByContractId(Guid contractId, CancellationToken cancellationToken)
    {
        
        return await _dbContext.Documents.SingleOrDefaultAsync(x => x.ContractId.Equals(contractId), cancellationToken);
    }

    public async Task UploadFile(DocumentFile file, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(file, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}