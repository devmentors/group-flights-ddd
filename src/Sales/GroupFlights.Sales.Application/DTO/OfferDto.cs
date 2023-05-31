using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.OfferDraft;

namespace GroupFlights.Sales.Application.DTO;

public record OfferDto(
    Guid Id,
    string OfferNumber,
    OfferSourceDto Source,
    ClientDto Client,
    RequestedTravelDto RequestedTravel,
    List<PriorityChoiceDto> Priorities,
    List<OfferVariantDto> Variants,
    DeadlineDto AcceptOfferDeadline,
    string Type,
    string CompletionStatus
);