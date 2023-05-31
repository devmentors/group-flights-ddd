using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Application.Commands.AcceptOffer;

public record AcceptOfferCommand(OfferId OfferId, AirlineOfferId VariantId) : ICommand;