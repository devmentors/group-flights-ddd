using GroupFlights.Inquiries.Core.DTO;
using GroupFlights.Inquiries.Core.Mapping;
using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Inquiries.Core.Repositories;
using GroupFlights.Inquiries.Core.Validators;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Plumbing.UserContext;
using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.WorkloadManagement.Shared;

namespace GroupFlights.Inquiries.Core.Services;

internal class InquiryService : IInquiryService
{
    private readonly IInquiryRepository _inquiryRepository;
    private readonly ISalesApi _salesApi;
    private readonly InquiryValidator _inquiryValidator;
    private readonly IWorkloadManagementApi _workloadManagementApi;
    private readonly IUserContextAccessor _userContextAccessor;
    private const string InquiryWorkloadType = nameof(Inquiry);

    public InquiryService(IInquiryRepository inquiryRepository,
        ISalesApi salesApi,
        InquiryValidator inquiryValidator,
        IWorkloadManagementApi workloadManagementApi,
        IUserContextAccessor userContextAccessor)
    {
        _inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
        _salesApi = salesApi ?? throw new ArgumentNullException(nameof(salesApi));
        _inquiryValidator = inquiryValidator ?? throw new ArgumentNullException(nameof(inquiryValidator));
        _workloadManagementApi = workloadManagementApi ?? throw new ArgumentNullException(nameof(workloadManagementApi));
        _userContextAccessor = userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));
    }
    
    public async Task SubmitInquiry(InquiryInputDto inquiryInput, CancellationToken cancellationToken = default)
    {
        await _inquiryValidator.Validate(inquiryInput);
        var inquiry = InquiryDtoToInquiry.Map(inquiryInput);
        await _inquiryRepository.Add(inquiry, cancellationToken);
    }

    public async Task<IReadOnlyCollection<InquiryDetailsDto>> BrowseInquiries(CancellationToken cancellationToken = default)
    {
        return (await _inquiryRepository.Browse(cancellationToken))
            .Select(inquiry => InquiryToInquiryDetailsDto.Map(inquiry))
            .ToList();
    }

    public async Task<InquiryDetailsDto> GetInquiryById(InquiryId inquiryId, CancellationToken cancellationToken = default)
    {
        var inquiry = await _inquiryRepository.GetById(inquiryId, cancellationToken);
        return InquiryToInquiryDetailsDto.Map(inquiry);
    }

    public async Task AcceptInquiry(InquiryId inquiryId, Guid? offerToCreateId = default, CancellationToken cancellationToken = default)
    {
        await EnsureCanWorkOnThisInquiry(inquiryId, cancellationToken);
        
        var inquiryData = await _inquiryRepository.GetById(inquiryId, cancellationToken);
        inquiryData.Accept(offerToCreateId);
        
        var request = InquiryToOfferDraftCommand.Map(inquiryData);
        await _salesApi.CreateOfferDraft(request, cancellationToken);
        await _inquiryRepository.Update(inquiryData, cancellationToken);
    }

    public async Task RejectInquiry(InquiryId inquiryId, string rejectionReason, CancellationToken cancellationToken = default)
    {
        await EnsureCanWorkOnThisInquiry(inquiryId, cancellationToken);
        
        var inquiryData = await _inquiryRepository.GetById(inquiryId, cancellationToken);
        inquiryData.Reject(rejectionReason);
        await _inquiryRepository.Update(inquiryData, cancellationToken);
    }

    private async Task EnsureCanWorkOnThisInquiry(InquiryId inquiryId, CancellationToken cancellationToken = default)
    {
        var isAssigned = await _workloadManagementApi.CanAccessWorkload(
            new (InquiryWorkloadType, inquiryId.Value.ToString(), _userContextAccessor.Get().CashierId),
            cancellationToken);

        if (isAssigned is false)
        {
            throw new NotAssignedToThisWorkloadException();
        }
    }
}