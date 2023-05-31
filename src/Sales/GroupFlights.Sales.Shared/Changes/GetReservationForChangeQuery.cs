using GroupFlights.Shared.Plumbing.Queries;

namespace GroupFlights.Sales.Shared.Changes;

public record GetReservationForChangeQuery(Guid ReservationId) : IQuery<ReservationToChangeDto>;