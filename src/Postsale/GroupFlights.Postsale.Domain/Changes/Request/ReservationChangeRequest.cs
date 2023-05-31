using GroupFlights.Postsale.Domain.Changes.Events;
using GroupFlights.Postsale.Domain.Changes.Outcome;
using GroupFlights.Postsale.Domain.Changes.Payments;
using GroupFlights.Postsale.Domain.Exceptions;
using GroupFlights.Postsale.Domain.Shared;
using GroupFlights.Postsale.Domain.Shared.Base;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Types;

namespace GroupFlights.Postsale.Domain.Changes.Request;

public class ReservationChangeRequest : DomainEventsSource
{
    private readonly ReservationToChange _reservationToChange;
    private readonly DateTime _newTravelDate;
    private readonly UserId _requester;
    private bool _isFeasible;
    private RequiredPayment _paymentRequiredToApplyChange;
    private ReservationCost _newCost;
    private List<FlightSegment> _newTravel;
    private ReservationChangeToApply _changeToApply;
    private bool _isActive;
    private CompletionStatus? _completionStatus;

    private ReservationChangeRequest()
    {
        
    }
    
    internal ReservationChangeRequest(ReservationToChange reservationToChange, DateTime newTravelDate, UserId requester, Guid? id = default)
    {
        if (reservationToChange.AirlineType is AirlineType.LowCost)
        {
            throw new ThisReservationDoesNotSupportChangesException();
        }
        
        _reservationToChange = reservationToChange ?? throw new ArgumentNullException(nameof(reservationToChange));
        _newTravelDate = newTravelDate;
        _requester = requester ?? throw new ArgumentNullException(nameof(requester));

        Id = id ?? Guid.NewGuid();

        _isActive = true;
    }
    
    public Guid Id { get; init; }

    public void SetUpChangeFeasibility(bool isFeasible, PaymentSetup paymentSetup, IReadOnlyCollection<FlightSegment> newTravel)
    {
        EnsureStillActive();
        
        _isFeasible = isFeasible;

        if (_isFeasible is false)
        {
            _domainEvents.Enqueue(new ReservationChangeFeasibilitySet(Id,
                _requester,
                _isFeasible,
                null,
                null,
                default,
                default));
            return;
        }
        
        _paymentRequiredToApplyChange = new RequiredPayment(
            paymentSetup.PaymentId,
            new Deadline(Guid.NewGuid(), paymentSetup.DueDate));

        var newTotalCost = _reservationToChange.CurrentCost.TotalCost + paymentSetup.Amount;
        _newCost = _reservationToChange.CurrentCost with { TotalCost = newTotalCost };

        if (newTravel.First().Date.Date != _newTravelDate.Date)
        {
            throw new ClientRequestedDifferentChangeDateThanProvidedException();
        }

        _newTravel = newTravel.ToList();
        
        _domainEvents.Enqueue(new ReservationChangeFeasibilitySet(Id,
            _requester,
            _isFeasible,
            _newCost,
            paymentSetup,
            _paymentRequiredToApplyChange.Deadline,
            _newTravel));
    }

    public void OnChangePayed()
    {
        EnsureStillActive();
        
        var paymentDeadlineFulfilled = _paymentRequiredToApplyChange.Deadline with { Fulfilled = true };
        _paymentRequiredToApplyChange = _paymentRequiredToApplyChange with { Deadline = paymentDeadlineFulfilled };

        var dateDifference = _newTravelDate - _reservationToChange.CurrentTravel.First().Date;
        
        var paymentsDeadlineChanges = _reservationToChange.CurrentPayments
            .Select(p => new PaymentDeadlineChange(
                p.PaymentId, 
                p.Deadline.DueDate.Add(dateDifference), 
                p.Deadline.Id))
            .ToList();

        var passengerNamesDeadlineChange = new PassengerNamesDeadlineChange(
            _reservationToChange.PassengerNamesDeadline.Id,
            _reservationToChange.PassengerNamesDeadline.DueDate.Add(dateDifference));

        _changeToApply = new ReservationChangeToApply(
            new TravelChange(_newTravel),
            _newCost,
            paymentsDeadlineChanges,
            passengerNamesDeadlineChange
        );
        
        _domainEvents.Enqueue(new ReservationChangeAccepted(
            Id, 
            _reservationToChange.ReservationId, 
            new TravelChange(_newTravel),
            _newCost,
            paymentsDeadlineChanges,
            passengerNamesDeadlineChange,
            _changeToApply));
    }

    public void OnPaymentOverdue()
    {
        _isActive = false;
        _completionStatus = CompletionStatus.ChangeRejectedOnPaymentOverdue;
        _domainEvents.Enqueue(new ReservationChangeRequestFinalized(Id, _completionStatus.Value));
    }

    public void RejectChange()
    {
        _isActive = false;
        _completionStatus = CompletionStatus.ChangeRejectedByRequester;
        _domainEvents.Enqueue(new ReservationChangeRequestFinalized(Id, _completionStatus.Value));
    }

    public void OnChangeApplied()
    {
        _isActive = false;
        _completionStatus = CompletionStatus.ChangeApplied;
        _domainEvents.Enqueue(new ReservationChangeRequestFinalized(Id, _completionStatus.Value));
    }


    private void EnsureStillActive()
    {
        if (_isActive is false)
        {
            throw new ChangeRequestIsNotActiveAnymoreException();
        }
    }

    public enum CompletionStatus
    {
        ChangeApplied,
        ChangeRejectedByRequester,
        ChangeRejectedOnPaymentOverdue
    }
}