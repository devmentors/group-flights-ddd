using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Domain.Changes.Outcome;

public record ReservationCost(Money TotalCost, Money RefundableCost, Guid Id)
{
    private ReservationCost() : this(default, default, default)
    {
        
    }
}