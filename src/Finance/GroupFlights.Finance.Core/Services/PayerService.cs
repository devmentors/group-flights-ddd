using GroupFlights.Finance.Core.Data;
using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Core.Models;
using GroupFlights.Finance.Core.Validators;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Finance.Core.Services;

internal class PayerService : IPayerService
{
    private readonly PayerValidator _payerValidator;
    private readonly FinanceDbContext _dbContext;

    public PayerService(PayerValidator payerValidator, FinanceDbContext dbContext)
    {
        _payerValidator = payerValidator ?? throw new ArgumentNullException(nameof(payerValidator));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }    

    public async Task AddPayer(Payer payer, CancellationToken cancellationToken)
    {
        if (payer.PayerId.Equals(Guid.Empty))
        {
            payer = payer with { PayerId = Guid.NewGuid() };
        }
        
        _payerValidator.Validate(payer);
        await _dbContext.Payers.AddAsync(payer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Payer>> GetPayersFor(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Payers.AsNoTracking().Where(p => p.UserId.Equals(userId)).ToListAsync(cancellationToken);
    }

    public async Task<Payer> GetPayerById(Guid payerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payers.AsNoTracking().SingleOrDefaultAsync(p => p.PayerId.Equals(payerId), cancellationToken);
    }
}