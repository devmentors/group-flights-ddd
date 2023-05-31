using GroupFlights.Sales.Domain.Offers;

namespace GroupFlights.Sales.Infrastructure.Data.Models;

internal class OfferDbModel : DbModel
{
    public static readonly string OfferDraftType = nameof(OfferDraft);
    public static readonly string OfferType = nameof(Offer);
    
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Object { get; set; }
}