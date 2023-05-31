using GroupFlights.Postsale.Domain.Exceptions;

namespace GroupFlights.Postsale.Domain.Changes.Outcome;

public class ReservationChangeToApply
{
    private readonly TravelChange _travelTravelChange;
    private readonly ReservationCost _costAfterChange;
    private readonly IReadOnlyCollection<PaymentDeadlineChange> _paymentDeadlineChanges;
    private readonly PassengerNamesDeadlineChange _passengerNamesDeadlineChange;

    private ReservationChangeToApply()
    {
    }
    
    public ReservationChangeToApply(
        TravelChange travelTravelChange,
        ReservationCost costAfterChange,
        IReadOnlyCollection<PaymentDeadlineChange> paymentDeadlineChanges,
        PassengerNamesDeadlineChange passengerNamesDeadlineChange)
    {
        if (travelTravelChange is null || travelTravelChange.NewTravelSegments.Count == 0)
        {
            throw new ThereIsNoChangeToBeAppliedException();
        }

        Id = Guid.NewGuid();
        _travelTravelChange = travelTravelChange;
        _costAfterChange = costAfterChange;
        _paymentDeadlineChanges = paymentDeadlineChanges;
        _passengerNamesDeadlineChange = passengerNamesDeadlineChange;
    }

    public Guid Id { get; init; }
}