using GroupFlights.Backoffice.Shared.DTO;
using GroupFlights.Shared.Types;

namespace GroupFlights.Backoffice.Core.Services;

internal class ConfigurationService : IConfigurationService
{
    public Task<CashierBufferConfigurationDto> GetCashierBufferConfiguration(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new CashierBufferConfigurationDto(3));
    }
    
    public Task<FeeConfigurationDto> GetFeeConfiguration(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new FeeConfigurationDto(
            new TicketFees(
                FeePerTicketOnShortHaulFlight: new Money(100, Currency.PLN), 
                FeePerTicketOnLongHaulFlight: new Money(150, Currency.PLN))));
    }

    public Task<SalesConfigurationDto> GetSalesConfiguration(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new SalesConfigurationDto(
            DefaultOfferValidTime: TimeSpan.FromHours(24),
            ContractSignTime: TimeSpan.FromHours(2)));
    }
}