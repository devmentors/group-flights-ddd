using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Shared.OfferDraft;

public record FlightDto(DateTime Date, Airport SourceAirport, Airport TargetAirport);