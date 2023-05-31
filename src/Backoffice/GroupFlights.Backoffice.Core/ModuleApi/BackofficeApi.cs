using GroupFlights.Backoffice.Shared;
using GroupFlights.Backoffice.Shared.DTO;
using GroupFlights.Shared.Types;

namespace GroupFlights.Backoffice.Core.ModuleApi;

internal class BackofficeApi : IBackofficeApi
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

    public Task<IReadOnlyCollection<UserId>> GetAvailableCashiersUserIds(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyCollection<UserId>>(new List<UserId>
        {
            new(Guid.Parse("6db85eb9-ffa5-42c2-b20c-0da756c89032")),
            new(Guid.NewGuid() /* Note: Losowy user dla testow notyfikacji >1 kasjera */)
        });
    }
}