using GroupFlights.Shared.Types.Commands;

namespace GroupFlights.Sales.Shared.OfferDraft.Commands;

public record SubmitOfferDraftCommand(
    OfferSourceDto Source,
    ClientDto Client,
    RequestedTravelDto RequestedTravel,
    IReadOnlyCollection<PriorityChoiceDto> Priorities,
    Guid? OfferId = null) : ICommand;