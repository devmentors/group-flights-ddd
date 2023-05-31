using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Shared.Plumbing.Queries;

namespace GroupFlights.Sales.Application.Queries.Reservations;

public class GetReservationsQueryHandler : IQueryHandler<GetReservationsQuery, List<ReservationDto>> 
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationsQueryHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
    }
    
    public async Task<List<ReservationDto>> HandleAsync(GetReservationsQuery query, CancellationToken cancellationToken = default)
    {
        var unconfirmed = (await _reservationRepository.BrowseUnconfirmedReservations(cancellationToken))
            .Select(UnconfirmedReservationMap.Map)
            .ToList();
        
        var reservations = (await _reservationRepository.BrowseReservations(cancellationToken))
            .Select(ReservationMap.Map)
            .ToList();
        
        return unconfirmed.Concat(reservations).ToList();
    }
}