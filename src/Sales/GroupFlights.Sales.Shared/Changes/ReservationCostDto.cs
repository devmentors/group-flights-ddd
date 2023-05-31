using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Shared.Changes;

public record ReservationCostDto(Money TotalCost, Money RefundableCost);