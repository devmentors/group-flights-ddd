using GroupFlights.Inquiries.Core.Data.EF;
using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Shared.Types.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Inquiries.Core.Repositories;

internal class InquiryRepository : IInquiryRepository
{
    private readonly InquiriesDbContext _dbContext;

    public InquiryRepository(InquiriesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task Add(Inquiry inquiry, CancellationToken cancellationToken = default)
    {
        await _dbContext.Inquiries.AddAsync(inquiry, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Inquiry> GetById(InquiryId inquiryId, CancellationToken cancellationToken = default)
    {
        var inquiry = await _dbContext.Inquiries.SingleOrDefaultAsync(x => x.Id.Equals(inquiryId), cancellationToken);
        
        if (inquiry is null)
        {
            throw new DoesNotExistException();
        }

        return inquiry;
    }

    public async Task<IReadOnlyCollection<Inquiry>> Browse(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Inquiries.ToListAsync(cancellationToken);
    }

    public async Task Update(Inquiry inquiry, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(inquiry);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}