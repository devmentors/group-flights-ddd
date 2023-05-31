namespace GroupFlights.Sales.Shared.OfferDraft;

public record OfferSourceDto(string SourceName, string SourceId)
{
    public const string InquirySourceName = "Inquiry";
}