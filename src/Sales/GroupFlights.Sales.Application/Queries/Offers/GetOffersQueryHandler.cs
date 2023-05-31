using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Shared.Plumbing.Queries;

namespace GroupFlights.Sales.Application.Queries.Offers;

internal class GetOffersQueryHandler : IQueryHandler<GetOffersQuery, List<OfferDto>>
{
    private readonly IOfferRepository _offerRepository;

    public GetOffersQueryHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
    }
    
    public async Task<List<OfferDto>> HandleAsync(GetOffersQuery query, CancellationToken cancellationToken = default)
    {
        var drafts = (await _offerRepository.BrowseDrafts(cancellationToken))
            .Select(OfferDraftMap.Map)
            .ToList();
        
        var offers = (await _offerRepository.BrowseOffers(cancellationToken))
            .Select(OfferMap.Map)
            .ToList();
        
        return drafts.Concat(offers).ToList();
    }
}