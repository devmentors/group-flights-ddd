using GroupFlights.Backoffice.Shared.DTO;

namespace GroupFlights.Backoffice.Core.Services;

public interface IConfigurationService
{
    Task<CashierBufferConfigurationDto> GetCashierBufferConfiguration(CancellationToken cancellationToken = default);
    Task<FeeConfigurationDto> GetFeeConfiguration(CancellationToken cancellationToken = default);
    Task<SalesConfigurationDto> GetSalesConfiguration(CancellationToken cancellationToken = default);
}