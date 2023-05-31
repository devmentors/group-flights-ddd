using System.Linq.Expressions;
using GroupFlights.Shared.Types.Specification;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Shared.Specifications;

internal class IsDeadlineOverdue : Specification<Deadline>
{
    private readonly IClock _clock;

    public IsDeadlineOverdue(IClock clock)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    protected override Expression<Func<Deadline, bool>> AsPredicateExpression()
    {
        return deadline => deadline.DueDate < _clock.UtcNow && (deadline.Fulfilled ?? false) == false;
    }
}