using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Application.DTO;

public record OfferVariantDto(
    AirlineType AirlineType,
    List<FlightSegmentDto> Travel,
    List<FlightSegmentDto> Return,
    string AirlineOfferId,
    DateTime ClientValidTo,
    bool PassengerNamesRequiredImmediately,
    bool AirportChargesAreRefundable,
    string ConfirmationLink,
    Money Cost);