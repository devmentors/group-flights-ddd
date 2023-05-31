using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Shared;

public class PresentableTravelCost
{
    private PresentableTravelCost() {}
    
    internal PresentableTravelCost(CalculatedTravelCost calculation, bool airportChargesAreRefundable)
        : this(
            totalCost: calculation.CalculateTotal(), 
            refundableCost: airportChargesAreRefundable 
                ? calculation.AirportCharges 
                : new Money(0, calculation.CalculateTotal().Currency))
    {}

    internal PresentableTravelCost(Money totalCost, Money refundableCost)
    {
        TotalCost = totalCost ?? throw new ArgumentNullException(nameof(totalCost));
        RefundableCost = refundableCost ?? throw new ArgumentNullException(nameof(refundableCost));
    }

    public Money TotalCost { get; init; }
    public Money RefundableCost { get; init; }
}