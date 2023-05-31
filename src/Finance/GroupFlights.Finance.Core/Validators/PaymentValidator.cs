using GroupFlights.Finance.Core.Exceptions;
using GroupFlights.Finance.Shared.SetupPayment;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Finance.Core.Validators;

internal class PaymentValidator
{
    private readonly IClock _clock;

    public PaymentValidator(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public void Validate(SetupPaymentDto paymentSetup)
    {
        if (paymentSetup is null)
        {
            throw new InvalidInputException(nameof(paymentSetup));
        }

        if (paymentSetup.PaymentId.Equals(Guid.Empty))
        {
            throw new InvalidInputException(nameof(paymentSetup.PaymentId));
        }

        if (paymentSetup.PayerId.Equals(Guid.Empty))
        {
            throw new InvalidInputException(nameof(paymentSetup.PayerId));
        }
        
        if (paymentSetup.Amount is null || paymentSetup.Amount.Amount <= 0)
        {
            throw new InvalidInputException(nameof(paymentSetup.Amount));
        }

        if (paymentSetup.DueDate <= _clock.UtcNow)
        {
            throw new InvalidInputException(nameof(paymentSetup.DueDate));
        }
    }
}