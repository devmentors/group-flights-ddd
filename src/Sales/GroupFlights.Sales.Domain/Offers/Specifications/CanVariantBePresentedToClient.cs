using System.Linq.Expressions;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Shared.Types.Specification;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Offers.Specifications;

public class CanVariantBePresentedToClient : Specification<OfferVariant>
{
    private readonly IClock _clock;

    public CanVariantBePresentedToClient(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    protected override Expression<Func<OfferVariant, bool>> AsPredicateExpression()
    {
        var isOverdue = new IsVariantOverdue(_clock);
        return variant => variant.HasBeenConfirmed && isOverdue.Check(variant) == false;
    }
}