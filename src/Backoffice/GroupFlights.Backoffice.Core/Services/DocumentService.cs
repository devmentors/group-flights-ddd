using GroupFlights.Backoffice.Core.DocumentGeneration;
using GroupFlights.Backoffice.Core.DTO;
using GroupFlights.Backoffice.Core.Models;
using GroupFlights.Backoffice.Core.Repositories;
using GroupFlights.Backoffice.Shared.IntegrationEvents;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Plumbing.UserContext;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Backoffice.Core.Services;

internal class DocumentService : IDocumentService
{
    private readonly ContractGenerator _contractGenerator;
    private readonly IFileRepository _fileRepository;
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IEventDispatcher _eventDispatcher;

    public DocumentService(ContractGenerator contractGenerator,
        IFileRepository fileRepository,
        IUserContextAccessor userContextAccessor,
        IEventDispatcher eventDispatcher)
    {
        _contractGenerator = contractGenerator ?? throw new ArgumentNullException(nameof(contractGenerator));
        _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
        _userContextAccessor = userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }
    
    public async Task GenerateContract(ContractGenerationRequestedIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        var fileBytes = await _contractGenerator.Generate(@event, cancellationToken);
        var fileId = Guid.NewGuid();
        await _fileRepository.UploadFile(new DocumentFile
        {
            FileId = fileId,
            Content = fileBytes,
            ContractId = @event.ContractId,
            Name = $"{fileId}.txt",
            Owner = @event.ContractSignee.UserId
        }, cancellationToken);
    }

    public async Task<DocumentFileDto> DownloadContractFile(Guid contractId, CancellationToken cancellationToken)
    {
        var document = await _fileRepository.GetFileByContractId(contractId, cancellationToken);
        return new DocumentFileDto(document.Content, document.Name);
    }

    public async Task UploadSignedContract(SubmitContractFileDto contractFileDto, CancellationToken cancellationToken)
    {
        var contractToSignExists =
            (await _fileRepository.GetFileByContractId(contractFileDto.ContractId, cancellationToken)) is not null;

        if (!contractToSignExists)
        {
            throw new DoesNotExistException();
        }
        
        var fileId = Guid.NewGuid();
        await _fileRepository.UploadFile(new DocumentFile
        {
            FileId = fileId,
            Content = contractFileDto.DocumentFile.Content,
            ContractId = @contractFileDto.ContractId,
            Name = contractFileDto.DocumentFile.FileName,
            Owner = _userContextAccessor.Get().UserId
        }, cancellationToken);

        var @event = new ContractSignedIntegrationEvent(contractFileDto.ContractId);
        await _eventDispatcher.PublishAsync(@event, cancellationToken);
    }
}