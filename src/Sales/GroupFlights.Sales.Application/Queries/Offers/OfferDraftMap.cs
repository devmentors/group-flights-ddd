using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.OfferDraft;

namespace GroupFlights.Sales.Application.Queries.Offers;

internal static class OfferDraftMap
{
    public static OfferDto Map(OfferDraft draft)
    {
        return new OfferDto(
            draft.Id.Value,
            draft.OfferNumber,
            new OfferSourceDto(draft.Source.SourceName, draft.Source.SourceId),
            new ClientDto(
                draft.Client.UserId?.ToString() ?? "", 
                draft.Client.Name, 
                draft.Client.Surname,
                draft.Client.Email.Value,
                draft.Client.PhoneNumber),
            CreateRequestedTravel(draft),
            CreatePriorities(draft),
            draft.Variants.Select(CreateVariant).ToList(),
            null,
            nameof(OfferDraft),
            null);
    }
    
    private static RequestedTravelDto CreateRequestedTravel(OfferDraft draft)
    {
        var declaredPassengers = new PassengersDataDto(
            InfantCount: draft.DeclaredPassengers.InfantCount,
            ChildrenCount: draft.DeclaredPassengers.ChildrenCount,
            AdultCount: draft.DeclaredPassengers.AdultCount);
        
        return new RequestedTravelDto(
            Travel: new FlightDto(
                Date: draft.RequestedTravel.Travel.Date,
                SourceAirport: draft.RequestedTravel.Travel.SourceAirport,
                TargetAirport: draft.RequestedTravel.Travel.TargetAirport),
            Return: new FlightDto(
                Date: draft.RequestedTravel.Return.Date,
                SourceAirport: draft.RequestedTravel.Return.SourceAirport,
                TargetAirport: draft.RequestedTravel.Return.TargetAirport),
            Passengers: declaredPassengers,
            CheckedBaggageRequired: draft.RequestedTravel.CheckedBaggageRequired,
            AdditionalServicesRequired: draft.RequestedTravel.AdditionalServicesRequired,
            Comments: draft.RequestedTravel.Comments);
    }
    
    private static List<PriorityChoiceDto> CreatePriorities(OfferDraft draft)
    {
        return draft.Priorities
            .Select(p => new PriorityChoiceDto((uint)p.Feature, p.Priority))
            .ToList();
    }

    private static OfferVariantDto CreateVariant(OfferVariant variant)
    {
        return new OfferVariantDto(
            variant.AirlineType,
            variant.Travel.Select(CreateFlightSegment).ToList(),
            variant.Return.Select(CreateFlightSegment).ToList(),
            variant.AirlineOfferId.Value,
            variant.ValidTo.ValidToForClient,
            variant.PassengerNamesRequiredImmediately,
            variant.AirportChargesAreRefundable,
            variant.ConfirmationLink?.ToString(),
            variant.Cost?.TotalCost);
    }

    private static FlightSegmentDto CreateFlightSegment(FlightSegment flightSegment)
    {
        return new FlightSegmentDto(flightSegment.Date,
            flightSegment.SourceAirport,
            flightSegment.TargetAirport,
            new FlightTimeDto(flightSegment.FlightTime.Hours, flightSegment.FlightTime.Minutes)
        );
    }
}