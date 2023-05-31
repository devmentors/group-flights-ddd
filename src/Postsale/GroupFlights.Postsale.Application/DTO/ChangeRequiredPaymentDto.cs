using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Application.DTO;

public record ChangeRequiredPaymentDto(Guid PayerId, Money Cost, ChangeDeadlineDto ChangeDeadline);