using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Reservations.Repositories;

public interface IReservationRepository
{
    Task AddUnconfirmedReservation(UnconfirmedReservation reservation, CancellationToken cancellationToken = default);
    Task<UnconfirmedReservation> GetUnconfirmedReservationById(ReservationId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<UnconfirmedReservation>> BrowseUnconfirmedReservations(CancellationToken cancellationToken = default);
    Task UpdateUnconfirmedReservation(UnconfirmedReservation reservation, CancellationToken cancellationToken = default);

    Task<UnconfirmedReservation> GetUnconfirmedReservationByContractId(Guid contractId, CancellationToken cancellationToken = default);
    Task ReplaceUnconfirmedWithConfirmedReservation(Reservation reservation, CancellationToken cancellationToken = default);
    Task<Reservation> GetReservationById(ReservationId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Reservation>> BrowseReservations(CancellationToken cancellationToken = default);
    Task UpdateReservation(Reservation reservation, CancellationToken cancellationToken = default);

}