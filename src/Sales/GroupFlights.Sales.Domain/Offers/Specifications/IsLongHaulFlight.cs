using System.Linq.Expressions;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Types.Specification;

namespace GroupFlights.Sales.Domain.Offers.Specifications;

public class IsLongHaulFlight : Specification<IReadOnlyCollection<FlightSegment>>
{
    public const ushort LongHaulTimeInMinutes = 6 * 60;
    
    protected override Expression<Func<IReadOnlyCollection<FlightSegment>, bool>> AsPredicateExpression()
    {
        return segments => segments.Any(_ => _.FlightTime.TotalMinutes > LongHaulTimeInMinutes);
    }
}