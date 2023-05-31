using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Application.Commands.AddOfferVariant;

public record AddOfferVariantCommand(
    Guid OfferId,
    AirlineType AirlineType,
    string AirlineName,
    IReadOnlyCollection<FlightSegmentDto> Travel,
    IReadOnlyCollection<FlightSegmentDto> Return,
    string AirlineOfferId,
    DateTime? ValidToFromAirlines = null) : ICommand;