using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Shared.Plumbing.Commands;
using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Application.Commands.RevealOffer;

public record RevealOfferCommand(OfferId OfferId) : ICommand;