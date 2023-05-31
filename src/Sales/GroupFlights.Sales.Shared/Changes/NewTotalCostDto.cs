using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Shared.Changes;

public record NewTotalCostDto(Money TotalCost, Money RefundableCost);