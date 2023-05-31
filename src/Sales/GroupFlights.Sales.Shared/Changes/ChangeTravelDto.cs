namespace GroupFlights.Sales.Shared.Changes;

public record ChangeTravelDto(
    IReadOnlyCollection<FlightSegmentDto> NewTravelSegments);