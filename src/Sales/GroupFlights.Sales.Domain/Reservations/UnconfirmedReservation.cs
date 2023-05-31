using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Reservations.Contract;
using GroupFlights.Sales.Domain.Reservations.Exceptions;
using GroupFlights.Sales.Domain.Reservations.Factories;
using GroupFlights.Sales.Domain.Reservations.Passengers;
using GroupFlights.Sales.Domain.Reservations.Specifications;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.DomainEvents;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;
using GroupFlights.Sales.Domain.Shared.Exceptions;
using GroupFlights.Sales.Domain.Shared.Specifications;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Reservations;

public class UnconfirmedReservation : DomainEventsSource
{
    private List<Passenger> _providedPassengers;
    
    private UnconfirmedReservation()
    {
    }
    
    public UnconfirmedReservation(
        Offer offer,
        AirlineOfferId variantId,
        PassengerNamesDeadlineFactory passengerNamesDeadlineFactory,
        ContractGenerationRequestFactory contractGenerationRequestFactory)
    {
        if (offer == null) throw new ArgumentNullException(nameof(offer));
        
        var variantChosenByClient = offer.Variants.SingleOrDefault(v => 
            v.AirlineOfferId.Equals(new AirlineOfferId(variantId)));
        
        if (variantChosenByClient == null) throw new ArgumentNullException(nameof(variantChosenByClient));

        Id = new ReservationId(Guid.NewGuid());
        SourceOfferId = offer.Id;
        AirlineOfferId = variantChosenByClient.AirlineOfferId;
        Client = offer.Client ?? throw new ArgumentNullException(nameof(offer.Client));
        DeclaredPassengers = offer.DeclaredPassengers ?? throw new ArgumentNullException(nameof(offer.DeclaredPassengers));
        PassengerNamesRequiredImmediately = variantChosenByClient.PassengerNamesRequiredImmediately;

        var passengerNamesDeadlineRequested = passengerNamesDeadlineFactory.Create(this, variantChosenByClient);
        PassengerNamesDeadline = passengerNamesDeadlineRequested.Deadline;
        _domainEvents.Enqueue(passengerNamesDeadlineRequested);

        ContractToSign = new ContractToSign(Guid.NewGuid());
        var contractGenerationRequested = contractGenerationRequestFactory.Create(this, variantChosenByClient);
        ContractToSignDeadline = contractGenerationRequested.Deadline;
        _domainEvents.Enqueue(contractGenerationRequested);
    }

    public ReservationId Id { get; private set; }
    public OfferId SourceOfferId { get; }
    public AirlineOfferId AirlineOfferId { get; init; }
    public Client Client { get; }
    public PassengersData DeclaredPassengers { get; }
    public bool PassengerNamesRequiredImmediately { get; init; }
    public Deadline PassengerNamesDeadline { get; set; }
    public IReadOnlyCollection<Passenger> ProvidedPassengers => _providedPassengers;

    public ContractToSign ContractToSign { get; private set; }
    public Deadline ContractToSignDeadline { get; private set; }
    public bool MarkedOverdue { get; private set; }


    public void OnContractSigned(Guid signedContractId, IClock clock)
    {
        if (new IsDeadlineOverdue(clock).Check(ContractToSignDeadline))
        {
            throw new DeadlineForUnconfirmedReservationOverdueException();
        }
        
        if (!ContractToSign.ContractId.Equals(signedContractId))
        {
            throw new SignedContractMismatchException();
        }
        
        ContractToSign = ContractToSign with { Signed = true };
        ContractToSignDeadline = ContractToSignDeadline with { Fulfilled = true };
        _domainEvents.Enqueue(new ReservationRequirementMetEvent(ContractToSignDeadline.Id));
    }

    public void OnContractDeadlineNotMet(IClock clock)
    {
        if (new IsDeadlineOverdue(clock).Check(ContractToSignDeadline) is false)
        {
            throw new DeadlineNotYetOverdueException();
        }
        
        ContractToSign = ContractToSign with { Signed = true };
        ContractToSignDeadline = ContractToSignDeadline with { Fulfilled = true };
        MarkedOverdue = true;
    }

    public void SetPassengersForFlight(IReadOnlyCollection<Passenger> passengers, IClock clock)
    {
        if (new IsDeadlineOverdue(clock).Check(PassengerNamesDeadline))
        {
            throw new DeadlineForUnconfirmedReservationOverdueException();
        }
        
        if (passengers.Count != DeclaredPassengers.TotalCount)
        {
            throw new ProvidedPassengersCountMustMeetDeclarationException();
        }
        
        _providedPassengers?.Clear();
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
        MarkedOverdue = true;
    }

    public bool CanConfirmReservation()
    {
        if (MarkedOverdue)
        {
            return false;
        }
        
        return new EverythingProvidedToConfirmReservation().Check(this);
    }
}