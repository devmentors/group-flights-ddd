using GroupFlights.Sales.Domain.Offers.Exceptions;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared;

namespace GroupFlights.Sales.Domain.Offers.Variants;

public class OfferVariant
{
    private OfferVariant()
    {
        
    }

    internal OfferVariant(
        AirlineType airlineType,
        string airlineName,
        IReadOnlyCollection<FlightSegment> travel,
        IReadOnlyCollection<FlightSegment> @return,
        AirlineOfferId airlineOfferId,
        ValidTo validTo,
        bool passengerNamesRequiredImmediately,
        bool airportChargesAreRefundable) : this()
    {
        AirlineOfferId = airlineOfferId ?? throw new ArgumentNullException(nameof(airlineOfferId));
        AirlineType = airlineType;
        AirlineName = airlineName;

        Travel = travel ?? throw new ArgumentNullException(nameof(travel));
        if (travel.Count == 0)
        {
            throw new VariantShouldConsistOfAtLeastOneFlightException(AirlineOfferId);
        }
        
        Return = @return;
        ValidTo = validTo ?? throw new ArgumentNullException(nameof(validTo));
        PassengerNamesRequiredImmediately = passengerNamesRequiredImmediately;
        AirportChargesAreRefundable = airportChargesAreRefundable;
    }
    
    public AirlineType AirlineType { get; init; }
    public string AirlineName { get; }
    public IReadOnlyCollection<FlightSegment> Travel { get; init; }
    public IReadOnlyCollection<FlightSegment> Return { get; init; }
    public AirlineOfferId AirlineOfferId { get; init; }
    public ValidTo ValidTo { get; private set; }
    public bool PassengerNamesRequiredImmediately { get; init; }
    public bool AirportChargesAreRefundable { get; init; }
    public Uri ConfirmationLink { get; internal set; }
    public CalculatedTravelCost DetailedCost { get; internal set; }
    public PresentableTravelCost Cost =>
        DetailedCost is not null ? new PresentableTravelCost(DetailedCost, AirportChargesAreRefundable) : null;

    public bool HasBeenConfirmed => DetailedCost is not null;
}