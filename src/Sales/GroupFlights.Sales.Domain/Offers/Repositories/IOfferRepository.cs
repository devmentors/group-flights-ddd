namespace GroupFlights.Sales.Domain.Offers.Repositories;

public interface IOfferRepository
{
    Task AddDraft(OfferDraft offerDraft, CancellationToken cancellationToken = default);
    Task<OfferDraft> GetDraftById(OfferId offerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OfferDraft>> BrowseDrafts(CancellationToken cancellationToken = default);
    Task UpdateDraft(OfferDraft offerDraft, CancellationToken cancellationToken = default);

    Task ReplaceDraftWithOffer(Offer offer, CancellationToken cancellationToken = default);
    Task<Offer> GetOfferById(OfferId offerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Offer>> BrowseOffers(CancellationToken cancellationToken = default);
    Task UpdateOffer(Offer offer, CancellationToken cancellationToken = default);
}