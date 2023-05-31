namespace GroupFlights.Sales.Shared.OfferDraft;

public record RequestedTravelDto(
    FlightDto Travel, 
    FlightDto Return, 
    PassengersDataDto Passengers, 
    bool CheckedBaggageRequired,
    bool AdditionalServicesRequired,
    string Comments);