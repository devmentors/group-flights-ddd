using GroupFlights.Shared.Types;

namespace GroupFlights.Finance.Core.Models;

internal record Payment(Guid PaymentId,
    Guid PayerId,
    Money Amount,
    DateTime DueDate,
    string PaymentGatewaySecret,
    bool Payed)
{
    private Payment() : this(default, default, default, default, default, default)
    {}
}