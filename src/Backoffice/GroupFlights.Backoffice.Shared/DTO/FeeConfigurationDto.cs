using GroupFlights.Shared.Types;

namespace GroupFlights.Backoffice.Shared.DTO;

public record FeeConfigurationDto(TicketFees TicketFees);

public record TicketFees(Money FeePerTicketOnShortHaulFlight, Money FeePerTicketOnLongHaulFlight);