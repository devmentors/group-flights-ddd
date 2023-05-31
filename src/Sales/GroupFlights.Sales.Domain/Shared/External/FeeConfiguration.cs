using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Shared.External;

public record FeeConfiguration(TicketFees TicketFees);

public record TicketFees(Money FeePerTicketOnShortHaulFlight, Money FeePerTicketOnLongHaulFlight);