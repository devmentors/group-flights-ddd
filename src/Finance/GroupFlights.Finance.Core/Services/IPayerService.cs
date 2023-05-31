using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Core.Models;

namespace GroupFlights.Finance.Core.Services;

public interface IPayerService
{
    Task AddPayer(Payer payer, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Payer>> GetPayersFor(Guid userId, CancellationToken cancellationToken = default);
    Task<Payer> GetPayerById(Guid payerId, CancellationToken cancellationToken = default);
}