using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Core.Models;
using GroupFlights.Finance.Shared.SetupPayment;

namespace GroupFlights.Finance.Core.External;

public interface IPaymentGatewayFacade
{
    Task SetUpPayment(SetupPaymentDto setupPayment, Payer payer, 
        string paymentGatewaySecret, CancellationToken cancellationToken = default);
}