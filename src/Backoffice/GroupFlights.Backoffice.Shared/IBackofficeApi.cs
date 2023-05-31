using GroupFlights.Backoffice.Shared.DTO;
using GroupFlights.Shared.Types;

namespace GroupFlights.Backoffice.Shared;

public interface IBackofficeApi
{
    Task<CashierBufferConfigurationDto> GetCashierBufferConfiguration(CancellationToken cancellationToken = default);
    Task<FeeConfigurationDto> GetFeeConfiguration(CancellationToken cancellationToken = default);
    Task<SalesConfigurationDto> GetSalesConfiguration(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UserId>> GetAvailableCashiersUserIds(CancellationToken cancellationToken = default);
}