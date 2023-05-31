using GroupFlights.Sales.Application.DTO;
using GroupFlights.Shared.Plumbing.Queries;

namespace GroupFlights.Sales.Application.Queries.Reservations;

public record GetReservationsQuery() : IQuery<List<ReservationDto>>;