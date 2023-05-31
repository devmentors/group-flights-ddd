using System.Linq.Expressions;
using GroupFlights.Sales.Domain.Reservations.Payments;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Specification;

namespace GroupFlights.Sales.Domain.Reservations.Specifications;

public class PaymentSetupCoversTravelCost : Specification<Reservation>
{
    private readonly PaymentSetup[] _attemptedPaymentsSetup;

    public PaymentSetupCoversTravelCost(PaymentSetup[] attemptedPaymentsSetup)
    {
        _attemptedPaymentsSetup = attemptedPaymentsSetup ?? throw new ArgumentNullException(nameof(attemptedPaymentsSetup));
    }
    
    protected override Expression<Func<Reservation, bool>> AsPredicateExpression()
    {
        Money totalAttemptedPayment = null;

        foreach (var payment in _attemptedPaymentsSetup)
        {
            if (totalAttemptedPayment is null)
            {
                totalAttemptedPayment = payment.Amount;
                continue;
            }

            totalAttemptedPayment += payment.Amount;
        }

        return reservation => reservation.Cost.TotalCost == totalAttemptedPayment;
    }
}