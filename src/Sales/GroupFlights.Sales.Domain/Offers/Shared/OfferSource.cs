namespace GroupFlights.Sales.Domain.Offers.Shared;

public class OfferSource
{
    public string SourceName { get; }
    public string SourceId { get; }

    internal OfferSource()
    {
        
    }
    
    public OfferSource(string sourceName, string sourceId)
    {
        SourceName = sourceName ?? throw new ArgumentNullException(nameof(sourceName));
        SourceId = sourceId ?? throw new ArgumentNullException(nameof(sourceId));
    }
    
    internal static OfferSource Inquiry(string sourceId) => new OfferSource("Inquiry", sourceId);
    internal static OfferSource OverdueOffer(string sourceId) => Offer(sourceId);
    internal static OfferSource RejectedOffer(string sourceId) => Offer(sourceId);
    internal static OfferSource UnchangeableReservation(string sourceId) => new("Reservation", sourceId);
    private static OfferSource Offer(string sourceId) => new OfferSource("Offer", sourceId);
}