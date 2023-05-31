using GroupFlights.Postsale.Domain.Exceptions;
using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Domain.Changes.Payments;

public class PaymentSetup
{
    private PaymentSetup(){}
    
    public PaymentSetup(Guid paymentId, Guid payerId, Money amount, DateTime dueDate)
    {
        if (paymentId.Equals(default))
        {
            throw new InvalidPaymentSetupException();
        }
        
        if (payerId.Equals(default))
        {
            throw new InvalidPaymentSetupException();
        }

        if (amount is null || amount.Amount <= 0)
        {
            throw new InvalidPaymentSetupException();
        }
        
        PaymentId = paymentId;
        PayerId = payerId;
        Amount = amount;
        DueDate = dueDate;
    }

    public Guid PaymentId { get; init; }
    public Guid PayerId { get; init; }
    public Money Amount { get; init; }
    public DateTime DueDate { get; init; }
}