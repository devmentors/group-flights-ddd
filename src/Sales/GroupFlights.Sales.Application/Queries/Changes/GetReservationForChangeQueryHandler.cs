using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Shared.Plumbing.Queries;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Application.Queries.Changes;

internal class GetReservationForChangeQueryHandler : IQueryHandler<GetReservationForChangeQuery, ReservationToChangeDto>
{
    private readonly IReservationRepository _repository;

    public GetReservationForChangeQueryHandler(IReservationRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task<ReservationToChangeDto> HandleAsync(GetReservationForChangeQuery query, CancellationToken cancellationToken = default)
    {
        var reservation = await _repository.GetReservationById(query.ReservationId);

        if (reservation is null)
        {
            throw new DoesNotExistException();
        }

        return new ReservationToChangeDto(
            reservation.Id,
            reservation.AirlineType,
            reservation.IsCompleted,
            new ReservationCostDto(reservation.Cost.TotalCost, reservation.Cost.RefundableCost),
            reservation.Travel.Select(CreateFlightSegment).ToList(),
            reservation.RequiredPayments?.Select(rp =>
                    new RequiredPaymentDto(
                        rp.PaymentId,
                        new DeadlineDto(rp.Deadline.DueDate, rp.Deadline.Fulfilled, rp.Deadline.Id.Value)))
                .ToList(),
            new DeadlineDto(
                reservation.PassengerNamesDeadline.DueDate,
                reservation.PassengerNamesDeadline.Fulfilled,
                reservation.PassengerNamesDeadline.Id.Value));
    }
    
    private static FlightSegmentDto CreateFlightSegment(FlightSegment flightSegment)
    {
        return new FlightSegmentDto(flightSegment.Date,
            flightSegment.SourceAirport,
            flightSegment.TargetAirport,
            new FlightTimeDto(flightSegment.FlightTime.Hours, flightSegment.FlightTime.Minutes)
        );
    }
}