using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Infrastructure.Data.EF;
using GroupFlights.Sales.Infrastructure.Data.Json;
using GroupFlights.Sales.Infrastructure.Data.Models;
using GroupFlights.Shared.Types.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Sales.Infrastructure.Data.Repositories;

internal class OfferRepository : IOfferRepository
{
    private readonly SalesDbContext _dbContext;

    public OfferRepository(SalesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task AddDraft(OfferDraft offerDraft, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Offers
            .AnyAsync(o => o.Id.Equals(offerDraft.Id.Value) && o.Type.Equals(OfferDbModel.OfferDraftType),
                cancellationToken);
        
        if (exists)
        {
            throw new AlreadyExistsException();
        }
        
        await _dbContext.Offers.AddAsync(new OfferDbModel
        {
            Id = offerDraft.Id,
            Object = ComplexObjectSerializer.SerializeToJson(offerDraft),
            Type = OfferDbModel.OfferDraftType
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<OfferDraft> GetDraftById(OfferId offerId, CancellationToken cancellationToken = default)
    {
        var offerDbModel = await _dbContext.Offers
            .SingleOrDefaultAsync(o => o.Id.Equals(offerId.Value) && o.Type.Equals(OfferDbModel.OfferDraftType),
                cancellationToken);
        
        if (offerDbModel is null)
        {
            throw new DoesNotExistException();
        }

        return ComplexObjectSerializer.DeserializeFromJson<OfferDraft>(offerDbModel.Object);
    }

    public async Task<IReadOnlyCollection<OfferDraft>> BrowseDrafts(CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Offers.Where(_ => _.Type.Equals(OfferDbModel.OfferDraftType)).ToListAsync(cancellationToken))
            .Select(o => ComplexObjectSerializer.DeserializeFromJson<OfferDraft>(o.Object))
            .ToList();
    }

    public async Task UpdateDraft(OfferDraft offerDraft, CancellationToken cancellationToken = default)
    {
        var existingDraft = await _dbContext.Offers.SingleOrDefaultAsync(
            o => o.Type.Equals(OfferDbModel.OfferDraftType), cancellationToken);
        
        if (existingDraft is null)
        {
            throw new DoesNotExistException();
        }

        existingDraft.Object = ComplexObjectSerializer.SerializeToJson(offerDraft);

        _dbContext.Offers.Update(existingDraft);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ReplaceDraftWithOffer(Offer offer, CancellationToken cancellationToken = default)
    {
        var existingDraft = await _dbContext.Offers.SingleOrDefaultAsync(
            o => o.Type.Equals(OfferDbModel.OfferDraftType), cancellationToken);
        
        if (existingDraft is null)
        {
            throw new DoesNotExistException();
        }

        existingDraft.Object = ComplexObjectSerializer.SerializeToJson(offer);
        existingDraft.Type = OfferDbModel.OfferType;

        _dbContext.Offers.Update(existingDraft);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Offer> GetOfferById(OfferId offerId, CancellationToken cancellationToken = default)
    {
        var offerDbModel = await _dbContext.Offers
            .SingleOrDefaultAsync(o => o.Id.Equals(offerId.Value) && o.Type.Equals(OfferDbModel.OfferType),
                cancellationToken);
        
        if (offerDbModel is null)
        {
            throw new DoesNotExistException();
        }

        return ComplexObjectSerializer.DeserializeFromJson<Offer>(offerDbModel.Object);
    }

    public async Task<IReadOnlyCollection<Offer>> BrowseOffers(CancellationToken cancellationToken = default)
    {
        return (await _dbContext.Offers.Where(_ => _.Type.Equals(OfferDbModel.OfferType)).ToListAsync(cancellationToken))
            .Select(o => ComplexObjectSerializer.DeserializeFromJson<Offer>(o.Object))
            .ToList();
    }

    public async Task UpdateOffer(Offer offer, CancellationToken cancellationToken = default)
    {
        var existingOffer = await _dbContext.Offers.SingleOrDefaultAsync(
            o => o.Type.Equals(OfferDbModel.OfferType), cancellationToken);
        
        if (existingOffer is null)
        {
            throw new DoesNotExistException();
        }

        existingOffer.Object = ComplexObjectSerializer.SerializeToJson(offer);

        _dbContext.Offers.Update(existingOffer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}