using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Changes.Payments;
using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Postsale.Domain.Changes.Request;
using GroupFlights.Postsale.Domain.Exceptions;
using GroupFlights.Postsale.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Domain.Changes.DomainService;

public class ReservationChangeRequestDomainService
{
    private readonly ISalesApi _salesApi;
    private readonly IReservationChangeRequestRepository _repository;

    public ReservationChangeRequestDomainService(
        ISalesApi salesApi,
        IReservationChangeRequestRepository repository)
    {
        _salesApi = salesApi ?? throw new ArgumentNullException(nameof(salesApi));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public async Task<ReservationChangeRequest> CreateChangeRequest(Guid reservationToChangeId,
        DateTime newTravelDate,
        UserId requester,
        Guid? changeRequestId = default,
        CancellationToken cancellationToken = default)
    {
        var reservationDto = await _salesApi.GetReservationForChange(new (reservationToChangeId), cancellationToken);

        if (reservationDto.CurrentPayments.Count == 0)
        {
            throw new ChangesArePossibleAfterFirstPaymentAppearsException();
        }
        
        var reservationToChange = MapToReservationToChange(reservationDto);
        
        var anyActiveChangeForThisReservation = await _repository.ExistsActiveForGivenUser(requester, cancellationToken);

        if (anyActiveChangeForThisReservation)
        {
            throw new OnlyOneActiveChangePerReservationIsAllowedException();
        }

        return new ReservationChangeRequest(reservationToChange, newTravelDate, requester, changeRequestId);
    }

    private static ReservationToChange MapToReservationToChange(ReservationToChangeDto reservationDto)
    {
        return new ReservationToChange(
            reservationDto.ReservationId,
            reservationDto.AirlineType,
            reservationDto.IsCompleted,
            new ReservationCost(reservationDto.CurrentCost.TotalCost, reservationDto.CurrentCost.RefundableCost, Guid.NewGuid()),
            MapSegments(reservationDto.CurrentTravel),
            reservationDto.CurrentPayments
                .Select(p =>
                    new RequiredPayment(p.PaymentId, new Deadline(p.Deadline.DeadlineId.Value, p.Deadline.DueDate)))
                .ToList(),
            new Deadline(reservationDto.PassengerNamesDeadline.DeadlineId.Value,
                reservationDto.PassengerNamesDeadline.DueDate));
    }

    private static List<FlightSegment> MapSegments(IReadOnlyCollection<FlightSegmentDto> segments)
    {
        return segments.Select(_ => new FlightSegment(_.Date, _.SourceAirport, _.TargetAirport, new FlightTime(_.FlightTime.Hours, _.FlightTime.Minutes))).ToList();
    }
}