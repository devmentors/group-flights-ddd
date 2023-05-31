using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Reservations.Changes;
using GroupFlights.Sales.Domain.Reservations.Exceptions;
using GroupFlights.Sales.Domain.Reservations.Factories;
using GroupFlights.Sales.Domain.Reservations.Passengers;
using GroupFlights.Sales.Domain.Reservations.Payments;
using GroupFlights.Sales.Domain.Reservations.Specifications;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;
using GroupFlights.Sales.Domain.Shared.Exceptions;
using GroupFlights.Sales.Domain.Shared.Specifications;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Reservations;

public class Reservation : DomainEventsSource
{
    private Reservation(){}
    
    private List<FlightSegment> _travel;
    private List<FlightSegment> _return;
    private List<Passenger> _providedPassengers;
    private List<RequiredPayment> _requiredPayments;

    public Reservation(UnconfirmedReservation reservation, OfferVariant variantChosenByClient,
        ConfirmInAirlinesDeadlineFactory confirmInAirlinesDeadlineFactory)
    {
        if (new EverythingProvidedToConfirmReservation().Check(reservation) is false)
        {
            throw new RequirementsNotMetToConfirmReservationException();
        }
        
        Id = reservation.Id;
        SourceOfferId = reservation.SourceOfferId;
        Client = reservation.Client ?? throw new ArgumentNullException(nameof(reservation.Client));
        DeclaredPassengers = reservation.DeclaredPassengers ?? throw new ArgumentNullException(nameof(reservation.DeclaredPassengers));
        _providedPassengers = (reservation.ProvidedPassengers ?? new List<Passenger>()).ToList();
        
        AirlineType = variantChosenByClient.AirlineType;
        AirlineName = variantChosenByClient.AirlineName;
        AirlineOfferId = variantChosenByClient.AirlineOfferId;
        
        _travel = variantChosenByClient.Travel.ToList();
        _return = variantChosenByClient.Return.ToList();
        PassengerNamesRequiredImmediately = variantChosenByClient.PassengerNamesRequiredImmediately;
        Cost = variantChosenByClient.Cost;

        PassengerNamesDeadline = reservation.PassengerNamesDeadline;

        var confirmInAirlinesDeadlineRequested = confirmInAirlinesDeadlineFactory.Create(this, variantChosenByClient);
        ConfirmInAirlinesDeadlines = confirmInAirlinesDeadlineRequested.Deadline;
        _domainEvents.Enqueue(confirmInAirlinesDeadlineRequested);
    }
    
    public ReservationId Id { get; private set; }
    public OfferId SourceOfferId { get; }
    public AirlineOfferId AirlineOfferId { get; init; }
    public Client Client { get; }
    public AirlineType AirlineType { get; private set; }
    public string AirlineName { get; private set; }
    public IReadOnlyCollection<FlightSegment> Travel => _travel;
    public IReadOnlyCollection<FlightSegment> Return => _return;
    
    public PassengersData DeclaredPassengers { get; }
    public bool PassengerNamesRequiredImmediately { get; init; }
    public Deadline PassengerNamesDeadline { get; private set; }
    public IReadOnlyCollection<Passenger> ProvidedPassengers => _providedPassengers;
    public PresentableTravelCost Cost { get; private set; }
    public bool CanChangePassengersOrTravel => AirlineType is AirlineType.Traditional;
    public Deadline ConfirmInAirlinesDeadlines { get; private set; }
    public IReadOnlyCollection<RequiredPayment> RequiredPayments => _requiredPayments;
    public CompletionStatus? Status { get; private set; }
    public bool IsCompleted => Status is not null;

    public void SetPassengersForFlight(IReadOnlyCollection<Passenger> passengers, IClock clock)
    {
        if (CanChangePassengersOrTravel is false)
        {
            throw new ThisReservationDoesNotAllowPassengerChangesException();
        }
        
        if (new IsDeadlineOverdue(clock).Check(PassengerNamesDeadline))
        {
            throw new DeadlineForUnconfirmedReservationOverdueException();
        }
        
        if (passengers.Count != DeclaredPassengers.TotalCount)
        {
            throw new ProvidedPassengersCountMustMeetDeclarationException();
        }
        
        _providedPassengers.Clear();
        _providedPassengers = passengers.ToList();
        PassengerNamesDeadline = PassengerNamesDeadline with { Fulfilled = true };
        _domainEvents.Enqueue(new ReservationRequirementMetEvent(PassengerNamesDeadline.Id));
    }
    
    public void OnPassengerNamesDeadlineNotMet(IClock clock)
    {
        if (new IsDeadlineOverdue(clock).Check(PassengerNamesDeadline) is false)
        {
            throw new DeadlineNotYetOverdueException();
        }
        PassengerNamesDeadline = PassengerNamesDeadline with { Fulfilled = true };
        Status = CompletionStatus.CanceledBecauseDeadlineNotMet;
    }

    public void SetUpPayments(PaymentSetup[] paymentsToSetup, IClock clock)
    {
        if (_requiredPayments is not null && _requiredPayments.Count > 0)
        {
            throw new PaymentAlreadySetUpForReservationException();
        }
        
        var anyPaymentOverdue = paymentsToSetup.Any(p => p.DueDate < clock.UtcNow);

        if (anyPaymentOverdue)
        {
            throw new CannotAcceptPaymentSetupThatIsAlreadyOverdueException();
        }

        if (new PaymentSetupCoversTravelCost(paymentsToSetup).Check(this) is false)
        {
            throw new PaymentSetupDoesNotExactlyCoverTotalCostException();
        }

        _requiredPayments = paymentsToSetup
            .Select(p => new RequiredPayment(
                p.PaymentId,
                new Deadline(new DeadlineId(Guid.NewGuid()), p.DueDate)))
            .ToList();

        var paymentEvents = new PaymentRequestedEventFactory(clock).Create(this, paymentsToSetup);

        foreach (var @event in paymentEvents)
        {
            _domainEvents.Enqueue(@event);
        }
        
        ConfirmInAirlinesDeadlines = ConfirmInAirlinesDeadlines with { Fulfilled = true };
        _domainEvents.Enqueue(new ReservationRequirementMetEvent(ConfirmInAirlinesDeadlines.Id));
    }

    public void OnPaymentPayed(Guid paymentId)
    {
        var paymentToMarkAsPayed = _requiredPayments.SingleOrDefault(p => p.PaymentId.Equals(paymentId));

        if (paymentToMarkAsPayed is null)
        {
            throw new DoesNotExistException();
        }

        _requiredPayments = _requiredPayments
            .Where(p => !p.PaymentId.Equals(paymentId))
            .Concat(new[] { paymentToMarkAsPayed with
            {
                Payed = true,
                Deadline = paymentToMarkAsPayed.Deadline with { Fulfilled = true } 
            } })
            .ToList();

        _domainEvents.Enqueue(new ReservationRequirementMetEvent(paymentToMarkAsPayed.Deadline.Id));
    }
    
    public void OnPaymentDeadlineNotMet(IClock clock, Guid paymentId)
    {
        var paymentToMarkOverdue = _requiredPayments.SingleOrDefault(p => p.PaymentId.Equals(paymentId));

        if (paymentToMarkOverdue is null)
        {
            throw new DoesNotExistException();
        }
        
        if (new IsDeadlineOverdue(clock).Check(paymentToMarkOverdue.Deadline) is false)
        {
            throw new DeadlineNotYetOverdueException();
        }
        
        _requiredPayments = _requiredPayments
            .Where(p => !p.PaymentId.Equals(paymentId))
            .Concat(new[] { paymentToMarkOverdue with
            {
                Payed = false,
                Deadline = paymentToMarkOverdue.Deadline with { Fulfilled = false } 
            } })
            .ToList();
        
        Status = CompletionStatus.CanceledBecauseDeadlineNotMet;
    }

    public void ApplyReservationChange(ReservationChange change, IClock clock)
    {
        EnsureCanChange();
        
        ApplyTravelChange();

        Cost = new PresentableTravelCost(totalCost: change.CostAfterChange.TotalCost,
            refundableCost: change.CostAfterChange.RefundableCost);

        if (change.PaymentDeadlineChanges.Any())
        {
            ApplyPaymentDeadlinesChanges();
        }

        if (PassengerNamesDeadline is not null)
        {

            if (!PassengerNamesDeadline.Id.Value.Equals(change.PassengerNamesDeadlineChange.DeadlineId))
            {
                throw new DeadlineMismatchException();
            }
            
            PassengerNamesDeadline = PassengerNamesDeadline with
            {
                DueDate = change.PassengerNamesDeadlineChange.NewDueDate
            };
        }

        _domainEvents.Enqueue(new ReservationChangesAppliedEvent(change.ReservationChangeRequestId));

        void ApplyTravelChange()
        {
            var travelChange = change.TravelTravelChange;

            var now = clock.UtcNow;
            var maxMonthsAhead = 12;
            var minDaysAhead = 2;

            var firstFlight = travelChange.NewTravelSegments.First();

            if (firstFlight.Date - now >= TimeSpan.FromDays(12 * maxMonthsAhead))
            {
                throw new FirstFlightTooFarInFutureException(maxMonthsAhead, firstFlight.Date);
            }

            if (firstFlight.Date - now < TimeSpan.FromDays(minDaysAhead))
            {
                throw new TooLateToOrganizeFlightException(minDaysAhead, firstFlight.Date);
            }

            _travel = travelChange.NewTravelSegments.ToList();
        }

        void ApplyPaymentDeadlinesChanges()
        {
            var newPayments = new List<RequiredPayment>();

            foreach (var payment in _requiredPayments)
            {
                var paymentChange = change.PaymentDeadlineChanges
                    .SingleOrDefault(p => p.PaymentId.Equals(payment.PaymentId));

                if (paymentChange == null)
                {
                    newPayments.Add(payment);
                    continue;
                }

                var newDeadline = payment.Deadline with { DueDate = paymentChange.NewDueDate };
                newPayments.Add(payment with { Deadline = newDeadline });
            }

            _requiredPayments = newPayments;
        }
    }

    private void EnsureCanChange()
    {
        if (CanChangePassengersOrTravel is false)
        {
            throw new ReservationChangesNotAllowedException();
        }
    }

    public bool CanIssueTickets(IClock clock)
    {
        var isOverdue = new IsDeadlineOverdue(clock);
        return RequiredPayments.Select(p => p.Deadline).Concat(new[] { PassengerNamesDeadline })
            .All(d => isOverdue.Check(d) is false)
            && RequiredPayments.All(p => p.Payed);
    }

    public void MarkTicketsIssued(IClock clock)
    {
        if (CanIssueTickets(clock) is false)
        {
            throw new CannotMarkTicketsIssuedIfRequirementsNotMetException();
        }
        
        Status = CompletionStatus.TicketsIssued;
    }

    public enum CompletionStatus
    {
        TicketsIssued,
        CanceledByClient,
        CanceledBecauseDeadlineNotMet
    }
}