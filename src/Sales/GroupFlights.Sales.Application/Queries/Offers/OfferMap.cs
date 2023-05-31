using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.OfferDraft;

namespace GroupFlights.Sales.Application.Queries.Offers;

internal static class OfferMap
{
    public static OfferDto Map(Offer offer)
    {
        return new OfferDto(
            offer.Id.Value,
            offer.OfferNumber,
            new OfferSourceDto(offer.Source.SourceName, offer.Source.SourceId),
            new ClientDto(
                offer.Client.UserId?.ToString() ?? "", 
                offer.Client.Name, 
                offer.Client.Surname,
                offer.Client.Email.Value,
                offer.Client.PhoneNumber),
            CreateRequestedTravel(offer),
            CreatePriorities(offer),
            offer.Variants.Select(CreateVariant).ToList(),
            new DeadlineDto(offer.AcceptOfferDeadline.DueDate, offer.AcceptOfferDeadline.Fulfilled),
            nameof(Offer),
            offer.Status?.ToString());
    }
    
    private static RequestedTravelDto CreateRequestedTravel(Offer offer)
    {
        var declaredPassengers = new PassengersDataDto(
            InfantCount: offer.DeclaredPassengers.InfantCount,
            ChildrenCount: offer.DeclaredPassengers.ChildrenCount,
            AdultCount: offer.DeclaredPassengers.AdultCount);
        
        return new RequestedTravelDto(
            Travel: new FlightDto(
                Date: offer.RequestedTravel.Travel.Date,
                SourceAirport: offer.RequestedTravel.Travel.SourceAirport,
                TargetAirport: offer.RequestedTravel.Travel.TargetAirport),
            Return: new FlightDto(
                Date: offer.RequestedTravel.Return.Date,
                SourceAirport: offer.RequestedTravel.Return.SourceAirport,
                TargetAirport: offer.RequestedTravel.Return.TargetAirport),
            Passengers: declaredPassengers,
            CheckedBaggageRequired: offer.RequestedTravel.CheckedBaggageRequired,
            AdditionalServicesRequired: offer.RequestedTravel.AdditionalServicesRequired,
            Comments: offer.RequestedTravel.Comments);
    }
    
    private static List<PriorityChoiceDto> CreatePriorities(Offer offer)
    {
        return new();
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