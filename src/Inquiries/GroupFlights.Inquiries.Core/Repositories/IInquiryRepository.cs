using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Shared.Types.Specification;

namespace GroupFlights.Inquiries.Core.Repositories;

internal interface IInquiryRepository
{
    Task Add(Inquiry inquiry, CancellationToken cancellationToken = default);
    Task<Inquiry> GetById(InquiryId inquiryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Inquiry>> Browse(CancellationToken cancellationToken = default);
    Task Update(Inquiry inquiry, CancellationToken cancellationToken = default);
}