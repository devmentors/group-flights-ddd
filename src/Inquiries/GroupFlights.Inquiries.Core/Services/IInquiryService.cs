using GroupFlights.Inquiries.Core.DTO;
using GroupFlights.Inquiries.Core.Models;

namespace GroupFlights.Inquiries.Core.Services;

internal interface IInquiryService
{
    Task SubmitInquiry(InquiryInputDto inquiryInput, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<InquiryDetailsDto>> BrowseInquiries(CancellationToken cancellationToken = default);
    Task<InquiryDetailsDto> GetInquiryById(InquiryId inquiryId, CancellationToken cancellationToken = default);
    Task AcceptInquiry(InquiryId inquiryId, Guid? offerIdToCreate = default, CancellationToken cancellationToken = default);
    Task RejectInquiry(InquiryId inquiryId, string rejectionReason, CancellationToken cancellationToken = default);
}