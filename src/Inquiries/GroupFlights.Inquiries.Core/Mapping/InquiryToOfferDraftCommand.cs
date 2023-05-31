using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Sales.Shared.OfferDraft;
using GroupFlights.Sales.Shared.OfferDraft.Commands;

namespace GroupFlights.Inquiries.Core.Mapping;

internal static class InquiryToOfferDraftCommand
{
    public static SubmitOfferDraftCommand Map(Inquiry inquiry)
    {
        return new SubmitOfferDraftCommand(
            CreateOfferSource(inquiry),
            CreateClient(inquiry),
            CreateRequestedTravel(inquiry),
            CreatePriorities(inquiry),
            inquiry.OfferId);
    }
    
    private static OfferSourceDto CreateOfferSource(Inquiry inquiry)
    {
        return new OfferSourceDto(
            SourceName: OfferSourceDto.InquirySourceName, 
            SourceId: inquiry.Id.Value.ToString());
    }

    private static ClientDto CreateClient(Inquiry inquiry)
    {
        return new ClientDto(
            UserId: inquiry.Inquirer.UserId?.ToString() ?? string.Empty,
            Name: inquiry.Inquirer.Name,
            Surname: inquiry.Inquirer.Surname,
            Email: inquiry.Inquirer.Email?.Value ?? string.Empty,
            PhoneNumber: inquiry.Inquirer.PhoneNumber);
    }

    private static RequestedTravelDto CreateRequestedTravel(Inquiry inquiry)
    {
        var declaredPassengers = new PassengersDataDto(
            InfantCount: inquiry.DeclaredPassengers.InfantCount,
            ChildrenCount: inquiry.DeclaredPassengers.ChildrenCount,
            AdultCount: inquiry.DeclaredPassengers.AdultCount);
        
        return new RequestedTravelDto(
            Travel: new FlightDto(
                Date: inquiry.Travel.Date,
                SourceAirport: inquiry.Travel.SourceAirport,
                TargetAirport: inquiry.Travel.TargetAirport),
            Return: new FlightDto(
                Date: inquiry.Return.Date,
                SourceAirport: inquiry.Return.SourceAirport,
                TargetAirport: inquiry.Return.TargetAirport),
            Passengers: declaredPassengers,
            CheckedBaggageRequired: inquiry.CheckedBaggageRequired,
            AdditionalServicesRequired: inquiry.AdditionalServicesRequired,
            Comments: inquiry.Comments);
    }
    
    private static IReadOnlyCollection<PriorityChoiceDto> CreatePriorities(Inquiry inquiry)
    {
        return inquiry.Priorities
            .Select(p => new PriorityChoiceDto((uint)p.Feature, p.Priority))
            .ToList();
    }
}