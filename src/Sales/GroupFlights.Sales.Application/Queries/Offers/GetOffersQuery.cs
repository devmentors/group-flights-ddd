using GroupFlights.Sales.Application.DTO;
using GroupFlights.Shared.Plumbing.Queries;

namespace GroupFlights.Sales.Application.Queries.Offers;

public record GetOffersQuery() : IQuery<List<OfferDto>>;