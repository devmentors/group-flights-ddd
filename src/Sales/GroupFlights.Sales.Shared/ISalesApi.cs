using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Sales.Shared.OfferDraft.Commands;

namespace GroupFlights.Sales.Shared;

public interface ISalesApi
{
    Task CreateOfferDraft(SubmitOfferDraftCommand request, CancellationToken cancellationToken = default);
    Task ChangeReservation(ChangeReservationCommand request, CancellationToken cancellationToken = default);
    Task<ReservationToChangeDto> GetReservationForChange(GetReservationForChangeQuery request, CancellationToken cancellationToken = default);
}