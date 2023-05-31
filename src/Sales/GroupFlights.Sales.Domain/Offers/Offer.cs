using GroupFlights.Communication.Shared.Models;
using GroupFlights.Sales.Domain.Offers.Exceptions;
using GroupFlights.Sales.Domain.Offers.Shared;
using GroupFlights.Sales.Domain.Offers.Specifications;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;
using GroupFlights.Sales.Domain.Shared.Exceptions;
using GroupFlights.Sales.Domain.Shared.Specifications;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Offers;

public class Offer : DomainEventsSource
{
    private List<OfferVariant> _variants = new();
    
    private Offer() {}
    
    internal Offer(OfferDraft draft, IClock clock)
    {
        Id = draft.Id;
        OfferNumber = draft.OfferNumber;
        Source = draft.Source;
        Client = draft.Client;
        DeclaredPassengers = draft.DeclaredPassengers;
        RequestedTravel = draft.RequestedTravel;
        
        var isOverdue = new IsVariantOverdue(clock);
        _variants = draft.Variants.Where(variant =>
                variant.HasBeenConfirmed &&
                isOverdue.Check(variant) is false)
            .ToList();

        var entireOfferValidTo = _variants.MaxBy(variant => variant.ValidTo.ValidToForClient).ValidTo.ValidToForClient;

        AcceptOfferDeadline = new Deadline(new DeadlineId(Guid.NewGuid()), entireOfferValidTo);
        _domainEvents.Enqueue(
            new OfferDeadlineRequestedEvent(
                AcceptOfferDeadline, 
                new Message($"Masz czas do {AcceptOfferDeadline.DueDate} na zapoznanie się z ofertą [{OfferNumber}] i zaakceptowanie lub odrzucenie jej."),
                this));
    }
    
    public OfferId Id { get; init; }
    public string OfferNumber { get; init; }
    public OfferSource Source { get; init; }
    public Client Client { get; init; }
    public PassengersData DeclaredPassengers { get; init; }
    public RequestedTravel RequestedTravel { get; init; }
    public IReadOnlyCollection<OfferVariant> Variants => _variants;
    public Deadline AcceptOfferDeadline { get; private set; }
    public CompletionStatus? Status { get; private set; }
    public bool IsCompleted => Status is not null;
    public string RejectReason { get; private set; }

    public void AcceptVariant(AirlineOfferId variantId, IClock clock, UserId acceptingUser)
    {
        EnsureOfferNotCompleted();
        
        var variant = Variants.SingleOrDefault(v => v.AirlineOfferId == variantId);
        if (variant is null)
        {
            throw new OfferDoesNotContainGivenVariant(variantId);
        }

        var isOverdue = new IsVariantOverdue(clock).Check(variant);
        var hasBeenConfirmed = variant.HasBeenConfirmed;

        if (isOverdue || hasBeenConfirmed is false)
        {
            throw new CannotAcceptVariantThatIsOverdueException(variant.AirlineOfferId);
        }
        
        if (acceptingUser is null)
        {
            throw new OfferCanOnlyBeAcceptedByRegisteredUserException();
        }

        Client.UserId = acceptingUser;
        Status = CompletionStatus.Accepted;
        AcceptOfferDeadline = AcceptOfferDeadline with { Fulfilled = true };
        _domainEvents.Enqueue(new OfferAcceptedEvent(this, variant));
    }

    public void RejectOffer(string reason)
    {
        EnsureOfferNotCompleted();

        Status = CompletionStatus.Rejected;
        RejectReason = reason;
        _domainEvents.Enqueue(new OfferRejectedEvent(Id, RejectReason));
    }

    public void OnAcceptOfferDeadlineNotMet(IClock clock)
    {
        EnsureOfferNotCompleted();
        if (new IsDeadlineOverdue(clock).Check(AcceptOfferDeadline) is false)
        {
            throw new DeadlineNotYetOverdueException();
        }
        AcceptOfferDeadline = AcceptOfferDeadline with { Fulfilled = false };
        Status = CompletionStatus.CanceledBecauseDeadlineNotMet;
    }

    private void EnsureOfferNotCompleted()
    {
        if (IsCompleted)
        {
            throw new OfferAlreadyCompletedException(this);
        }
    }

    public enum CompletionStatus
    {
        Accepted,
        Rejected,
        CanceledBecauseDeadlineNotMet
    }
}