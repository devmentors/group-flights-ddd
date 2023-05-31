using GroupFlights.Backoffice.Shared.DTO;
using GroupFlights.Sales.Domain.Offers.Specifications;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Shared.Types;
using TicketFees = GroupFlights.Sales.Domain.Shared.External.TicketFees;

namespace GroupFlights.Sales.Domain.Offers.Policies;

public class HaulBasedTicketFeePolicy
{
    public CalculatedTravelCost CalculateCost(
        OfferDraft draft, 
        OfferVariant variant, 
        Money airportCharges,
        Money netPrice,
        TicketFees ticketFeesConfiguration)
    {
        var ticketFeeAmount = new IsLongHaulFlight().Check(variant.Travel)
            ? ticketFeesConfiguration.FeePerTicketOnLongHaulFlight
            : ticketFeesConfiguration.FeePerTicketOnShortHaulFlight;

        var ticketsProfitFee = ticketFeeAmount * draft.DeclaredPassengers.TotalCount;
        return new CalculatedTravelCost(airportCharges, netPrice, ticketsProfitFee);
    }
}