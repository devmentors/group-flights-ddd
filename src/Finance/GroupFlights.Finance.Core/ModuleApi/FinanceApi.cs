using GroupFlights.Finance.Core.Services;
using GroupFlights.Finance.Shared;
using GroupFlights.Finance.Shared.SetupPayment;

namespace GroupFlights.Finance.Core.ModuleApi;

internal class FinanceApi : IFinanceApi
{
    private readonly IPaymentService _paymentService;

    public FinanceApi(IPaymentService paymentService)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    }
    
    public async Task SetupPayment(SetupPaymentDto dto, CancellationToken cancellationToken)
    {
        await _paymentService.SetupPayment(dto, cancellationToken);
    }
}