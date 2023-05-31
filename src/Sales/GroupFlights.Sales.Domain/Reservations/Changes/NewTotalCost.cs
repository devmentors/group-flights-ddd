using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Reservations.Changes;

public record NewTotalCost(Money TotalCost, Money RefundableCost);