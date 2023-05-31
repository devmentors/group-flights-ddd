using GroupFlights.Sales.Domain.Shared.Exceptions;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Sales.Domain.Shared;

public class RequestedTravel
{
    private RequestedTravel()
    {
        
    }
    
    public RequestedTravel(Flight travel, 
        Flight @return, 
        PassengersData passengers, 
        bool checkedBaggageRequired,
        bool additionalServicesRequired,
        string comments,
        IClock clock)
    {
        Travel = travel ?? throw new ArgumentNullException(nameof(travel));
        Return = @return;
        Passengers = passengers ?? throw new ArgumentNullException(nameof(passengers));
        CheckedBaggageRequired = checkedBaggageRequired;
        AdditionalServicesRequired = additionalServicesRequired;
        Comments = comments;
        
        EnsureFirstFlightIsInFuture(clock);
        EnsureFirstFlightNotTooFarFromNow(clock);
    }

    public Flight Travel { get; init; }
    public Flight Return { get; init; }
    public PassengersData Passengers { get; init; }
    public bool CheckedBaggageRequired { get; init; }
    public bool AdditionalServicesRequired { get; init; }
    public string Comments { get; init; }
    
    private void EnsureFirstFlightNotTooFarFromNow(IClock clock)
    {
        var now = clock.UtcNow;
        var maxMonthsAhead = 12;

        if (Travel.Date - now >= TimeSpan.FromDays(12 * maxMonthsAhead))
        {
            throw new FirstFlightTooFarInFutureException(maxMonthsAhead, Travel.Date);
        }
    }

    private void EnsureFirstFlightIsInFuture(IClock clock)
    {
        var now = clock.UtcNow;
        var minDaysAhead = 2;
        
        if (Travel.Date - now < TimeSpan.FromDays(minDaysAhead))
        {
            throw new TooLateToOrganizeFlightException(minDaysAhead, Travel.Date);
        }
    }
    
    public class Flight
    {
        public Flight(DateTime date, Airport sourceAirport, Airport targetAirport)
        {
            Date = date;
            SourceAirport = sourceAirport ?? throw new ArgumentNullException(nameof(sourceAirport));
            TargetAirport = targetAirport ?? throw new ArgumentNullException(nameof(targetAirport));
        }

        public DateTime Date { get; init; }
        public Airport SourceAirport { get; init; }
        public Airport TargetAirport { get; init; }
    }
}