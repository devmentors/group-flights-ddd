using GroupFlights.Sales.Domain.Reservations.Exceptions;

namespace GroupFlights.Sales.Domain.Reservations.Changes;

public class ReservationChange
{
    private ReservationChange()
    {
    }
    
    public ReservationChange(Guid reservationChangeRequestId,
        TravelChange travelTravelChange,
        NewTotalCost costAfterChange,
        IReadOnlyCollection<PaymentDeadlineChange> paymentDeadlineChanges,
        PassengerNamesDeadlineChange passengerNamesDeadlineChange)
    {
        if (reservationChangeRequestId.Equals(default))
        {
            throw new ArgumentNullException(nameof(reservationChangeRequestId));
        }

        if (travelTravelChange is null || travelTravelChange.NewTravelSegments.Count == 0)
        {
            throw new ThereIsNoChangeToBeAppliedException();
        }

        ReservationChangeRequestId = reservationChangeRequestId;
        TravelTravelChange = travelTravelChange;
        CostAfterChange = costAfterChange;
        PaymentDeadlineChanges = paymentDeadlineChanges;
        PassengerNamesDeadlineChange = passengerNamesDeadlineChange;
    }

    public Guid ReservationChangeRequestId { get; init; }
    public TravelChange TravelTravelChange { get; init; }
    public NewTotalCost CostAfterChange { get; init; }
    public IReadOnlyCollection<PaymentDeadlineChange> PaymentDeadlineChanges { get; init; }
    public PassengerNamesDeadlineChange PassengerNamesDeadlineChange { get; init; }
}