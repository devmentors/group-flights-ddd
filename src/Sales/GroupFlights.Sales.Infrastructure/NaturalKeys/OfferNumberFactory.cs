using GroupFlights.Sales.Application.NaturalKeys;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;

namespace GroupFlights.Sales.Infrastructure.NaturalKeys;

internal class OfferNumberFactory : INaturalKeyFactory<OfferDraft>
{
    private readonly IOfferRepository _offerRepository;

    public OfferNumberFactory(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
    }

    public string CreateNaturalKey()
    {
        var offerNumber = _offerRepository.BrowseOffers(default)
            .GetAwaiter().GetResult().Count;

        return $"OFR/{offerNumber+1}";
    }
}