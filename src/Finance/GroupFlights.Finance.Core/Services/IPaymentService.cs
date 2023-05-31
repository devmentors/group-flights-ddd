using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Shared.SetupPayment;

namespace GroupFlights.Finance.Core.Services;

public interface IPaymentService
{
    Task SetupPayment(SetupPaymentDto paymentSetup, CancellationToken cancellationToken = default);
    Task OnPaymentPayed(PaymentWebhookDto webhookDto, CancellationToken cancellationToken = default);
}