using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Postsale.Application.Commands.RequestReservationChange;

public record RequestReservationChangeCommand(Guid ReservationToChangeId, DateTime NewTravelDate, Guid? ChangeRequestId) : ICommand;