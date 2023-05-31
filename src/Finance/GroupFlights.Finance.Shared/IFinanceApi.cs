using GroupFlights.Finance.Shared.SetupPayment;

namespace GroupFlights.Finance.Shared;

public interface IFinanceApi
{
    Task SetupPayment(SetupPaymentDto dto, CancellationToken cancellationToken);
}