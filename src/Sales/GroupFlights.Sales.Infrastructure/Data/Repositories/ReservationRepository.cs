using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Sales.Infrastructure.Data.EF;
using GroupFlights.Sales.Infrastructure.Data.Json;
using GroupFlights.Sales.Infrastructure.Data.Models;
using GroupFlights.Shared.Types.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Sales.Infrastructure.Data.Repositories;

internal class ReservationRepository : IReservationRepository
{
    private readonly SalesDbContext _dbContext;

    public ReservationRepository(SalesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task AddUnconfirmedReservation(UnconfirmedReservation reservation, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Reservations
            .AnyAsync(o => o.Id.Equals(reservation.Id.Value) 
                           && o.Type.Equals(ReservationDbModel.UnconfirmedReservationType),
                cancellationToken);
        
        if (exists)
        {
            throw new AlreadyExistsException();
        }
        
        await _dbContext.Reservations.AddAsync(new ReservationDbModel
        {
            Id = reservation.Id.Value,
            Object = ComplexObjectSerializer.SerializeToJson(reservation),
            Type = ReservationDbModel.UnconfirmedReservationType,
            ContractId = reservation.ContractToSign?.ContractId
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UnconfirmedReservation> GetUnconfirmedReservationById(ReservationId id, CancellationToken cancellationToken = default)
    {
        var reservationDbModel = await _dbContext.Reservations.SingleOrDefaultAsync(
            o => o.Id.Equals(id.Value) && o.Type.Equals(ReservationDbModel.UnconfirmedReservationType),
                cancellationToken);
        
        if (reservationDbModel is null)
        {
            throw new DoesNotExistException();
        }

        return ComplexObjectSerializer.DeserializeFromJson<UnconfirmedReservation>(reservationDbModel.Object);
    }

    public async Task<IReadOnlyCollection<UnconfirmedReservation>> BrowseUnconfirmedReservations(CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Reservations.Where(_ => _.Type.Equals(ReservationDbModel.UnconfirmedReservationType)).ToListAsync(cancellationToken))
            .Select(o => ComplexObjectSerializer.DeserializeFromJson<UnconfirmedReservation>(o.Object))
            .ToList();
    }

    public async Task UpdateUnconfirmedReservation(UnconfirmedReservation reservation, CancellationToken cancellationToken = default)
    {
        var existingReservation = await _dbContext.Reservations.SingleOrDefaultAsync(
            o => o.Type.Equals(ReservationDbModel.UnconfirmedReservationType), cancellationToken);
        
        if (existingReservation is null)
        {
            throw new DoesNotExistException();
        }

        existingReservation.Object = ComplexObjectSerializer.SerializeToJson(reservation);
        existingReservation.ContractId = reservation.ContractToSign?.ContractId;

        _dbContext.Reservations.Update(existingReservation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UnconfirmedReservation> GetUnconfirmedReservationByContractId(Guid contractId, CancellationToken cancellationToken = default)
    {
        var reservationDbModel = await _dbContext.Reservations.Where(_ => _.Type.Equals(ReservationDbModel.UnconfirmedReservationType))
                .Where(_ => _.ContractId.Equals(contractId)).SingleOrDefaultAsync(cancellationToken);
        
        return ComplexObjectSerializer.DeserializeFromJson<UnconfirmedReservation>(reservationDbModel.Object);
    }

    public async Task ReplaceUnconfirmedWithConfirmedReservation(Reservation reservation, CancellationToken cancellationToken = default)
    {
        var existingReservation = await _dbContext.Reservations.SingleOrDefaultAsync(
            o => o.Type.Equals(ReservationDbModel.UnconfirmedReservationType), cancellationToken);
        
        if (existingReservation is null)
        {
            throw new DoesNotExistException();
        }

        existingReservation.Object = ComplexObjectSerializer.SerializeToJson(reservation);
        existingReservation.Type = ReservationDbModel.ReservationType;

        _dbContext.Reservations.Update(existingReservation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Reservation> GetReservationById(ReservationId id, CancellationToken cancellationToken = default)
    {
        var reservationDbModel = await _dbContext.Reservations
            .SingleOrDefaultAsync(o => o.Id.Equals(id.Value) && o.Type.Equals(ReservationDbModel.ReservationType),
                cancellationToken);
        
        if (reservationDbModel is null)
        {
            throw new DoesNotExistException();
        }

        return ComplexObjectSerializer.DeserializeFromJson<Reservation>(reservationDbModel.Object);
    }

    public async Task<IReadOnlyCollection<Reservation>> BrowseReservations(CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Reservations.Where(_ => _.Type.Equals(ReservationDbModel.ReservationType)).ToListAsync(cancellationToken))
            .Select(o => ComplexObjectSerializer.DeserializeFromJson<Reservation>(o.Object))
            .ToList();
    }

    public async Task UpdateReservation(Reservation reservation, CancellationToken cancellationToken = default)
    {
        var existingReservation = await _dbContext.Reservations.SingleOrDefaultAsync(
            o => o.Type.Equals(ReservationDbModel.ReservationType), cancellationToken);
        
        if (existingReservation is null)
        {
            throw new DoesNotExistException();
        }

        existingReservation.Object = ComplexObjectSerializer.SerializeToJson(reservation);

        _dbContext.Reservations.Update(existingReservation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}