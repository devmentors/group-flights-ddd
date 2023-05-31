using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Application.Commands.SetUpPaymentOnReservation;

public record SetUpPaymentOnReservationCommand(Guid ReservationId, PaymentToSetupDto[] PaymentsToSetup) : ICommand;

public record PaymentToSetupDto(Guid PaymentId, Guid PayerId, Money Amount, DateTime DueDate);