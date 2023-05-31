using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Application.Commands.ConfirmVariant;

public record ConfirmOfferVariantCommand(
    Guid OfferId,
    string AirlineOfferId, 
    Money AirportCharges, 
    Money NetPrice, 
    Uri ConfirmationLink = default) : ICommand;