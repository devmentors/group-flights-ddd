using System.Linq.Expressions;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Shared.Types.Specification;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Offers.Specifications;

public class IsVariantOverdue : Specification<OfferVariant>
{
    private readonly IClock _clock;

    public IsVariantOverdue(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    protected override Expression<Func<OfferVariant, bool>> AsPredicateExpression()
    {
        return variant => variant.ValidTo.ValidToForClient < _clock.UtcNow;
    }
}