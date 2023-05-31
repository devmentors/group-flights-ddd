using GroupFlights.Sales.Domain.Offers.Exceptions;
using GroupFlights.Sales.Domain.Offers.Policies;
using GroupFlights.Sales.Domain.Offers.Shared;
using GroupFlights.Sales.Domain.Offers.Specifications;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.Exceptions;
using GroupFlights.Sales.Domain.Shared.External;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Offers;

public class OfferDraft
{
    private List<OfferVariant> _variants = new();
    
    private OfferDraft(){}

    public OfferDraft(
        string offerNumber,
        OfferSource source,
        Client client,
        RequestedTravel requestedTravel,
        IReadOnlyCollection<PriorityChoice> priorities,
        OfferId id = default)
    {
        if (string.IsNullOrEmpty(offerNumber))
        {
            throw new ArgumentNullException(nameof(offerNumber));
        }
        OfferNumber = offerNumber;
        
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Client = client ?? throw new ArgumentNullException(nameof(client));
        DeclaredPassengers = requestedTravel?.Passengers ?? throw new ArgumentNullException(nameof(requestedTravel.Passengers));
        RequestedTravel = requestedTravel ?? throw new ArgumentNullException(nameof(requestedTravel));
        Priorities = priorities ?? throw new ArgumentNullException(nameof(priorities));
        Id = id ?? new OfferId(Guid.NewGuid());
    }

    public OfferId Id { get; init; }
    public string OfferNumber { get; init; }
    public OfferSource Source { get; init; }
    public Client Client { get; init; }
    public PassengersData DeclaredPassengers { get; init; }
    public RequestedTravel RequestedTravel { get; init; }
    public IReadOnlyCollection<PriorityChoice> Priorities { get; init; }
    public IReadOnlyCollection<OfferVariant> Variants => _variants;

    public void AddNewVariant(
        AirlineType airlineType,
        string airlineName,
        IReadOnlyCollection<FlightSegment> travel,
        IReadOnlyCollection<FlightSegment> @return,
        AirlineOfferId variantId,
        IClock clock,
        SalesConfiguration salesConfiguration,
        CashierBufferConfiguration cashierBuffer,
        DateTime? validToFromAirlines = null)
    {
        var passengerNamesRequiredImmediately = airlineType is AirlineType.LowCost;
        var airportChargesAreRefundable = airlineType is AirlineType.Traditional;
        
        var now = clock.UtcNow;
        
        if (validToFromAirlines is null && airlineType is AirlineType.Traditional)
        {
            throw new TraditionalAirlinesVariantShouldProvideValidToDate();
        }
        
        var defaultOfferValidTime = salesConfiguration.DefaultOfferValidTime;
        var validToFromAirlinesResolved = airlineType is AirlineType.Traditional
            ? validToFromAirlines.Value
            : now.Add(defaultOfferValidTime);
        
        var validTo = new ValidTo(
            ValidToInAirlines: validToFromAirlinesResolved,
            ValidToForClient: validToFromAirlinesResolved.Subtract(
                TimeSpan.FromHours(cashierBuffer.MinimalBufferInHours)));
        

        var variant = new OfferVariant(
            airlineType,
            airlineName,
            travel,
            @return,
            variantId,
            validTo,
            passengerNamesRequiredImmediately,
            airportChargesAreRefundable);
        
        if (new IsVariantOverdue(clock).Check(variant))
        {
            throw new CannotAddAlreadyOverdueVariant();
        }

        if (_variants.Exists(v => v.AirlineOfferId.Equals(variant.AirlineOfferId)))
        {
            throw new CannotOfferSameVariantTwiceException(variant.AirlineOfferId);
        }
        
        _variants.Add(variant);
    }
    
    public void ConfirmVariant(
        AirlineOfferId variantId,
        Money airportCharges,
        Money netPrice,
        Uri confirmationLink,
        FeeConfiguration feeConfiguration)
    {
        var variant = Variants.SingleOrDefault(v => v.AirlineOfferId == variantId);
        if (variant is null)
        {
            throw new OfferDoesNotContainGivenVariant(variantId);
        }

        if (variant.HasBeenConfirmed)
        {
            throw new VariantAlreadyConfirmedException(variantId);
        }
        
        var ticketFeesConfig = feeConfiguration.TicketFees;
        var calculatedTravelCost = new HaulBasedTicketFeePolicy().CalculateCost(this, variant, airportCharges, netPrice, ticketFeesConfig);
        
        if (confirmationLink == null && variant.AirlineType is AirlineType.LowCost) throw new ArgumentNullException(nameof(confirmationLink));

        variant.DetailedCost = calculatedTravelCost;
        variant.ConfirmationLink = confirmationLink;
    }

    internal void RemoveVariant(AirlineOfferId variantId)
    {
        _variants = _variants.Where(_ => _.AirlineOfferId != variantId).ToList();
    }

    public Offer RevealToClient(IClock clock)
    {
        var canBePresented = new CanVariantBePresentedToClient(clock);
        var anyVariantValid = _variants.Any(variant => canBePresented.Check(variant));

        if (anyVariantValid is false)
        {
            throw new CannotRevealOfferThatHasAllVariantsOverdue();
        }

        return new Offer(this, clock);
    }
}