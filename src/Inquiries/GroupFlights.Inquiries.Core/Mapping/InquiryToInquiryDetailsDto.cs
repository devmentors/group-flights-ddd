using GroupFlights.Inquiries.Core.DTO;
using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Shared.Types;

namespace GroupFlights.Inquiries.Core.Mapping;

internal static class InquiryToInquiryDetailsDto
{
    public static InquiryDetailsDto Map(Inquiry inquiry)
    {
        return new InquiryDetailsDto(
            Id: inquiry.Id.Value.ToString(),
            Inquirer: CreateInquirer(inquiry),
            Travel: CreateTravel(inquiry),
            Return: CreateReturn(inquiry),
            DeclaredInquiryPassengers: CreatePassengers(inquiry),
            Priorities: CreatePriorities(inquiry),
            CheckedBaggageRequired: inquiry.CheckedBaggageRequired,
            AdditionalServicesRequired: inquiry.AdditionalServicesRequired,
            Comments: inquiry.Comments,
            VerificationResult: inquiry.VerificationResult,
            RejectionReason: inquiry.RejectionReason);
    }
    
    private static InquirerDto CreateInquirer(Inquiry inquiry)
    {
        return new InquirerDto(
            UserId: inquiry.Inquirer.UserId?.Value.ToString() ?? string.Empty,
            Name: inquiry.Inquirer.Name,
            Surname: inquiry.Inquirer.Surname,
            Email: inquiry.Inquirer.Email.Value,
            PhoneNumber: inquiry.Inquirer.PhoneNumber);
    }

    private static InquiryFlightDto CreateTravel(Inquiry inquiry)
    {
        return new InquiryFlightDto(
            Date: inquiry.Travel.Date,
            SourceAirport: inquiry.Travel.SourceAirport,
            TargetAirport: inquiry.Travel.TargetAirport);
    }
    private static InquiryFlightDto CreateReturn(Inquiry inquiry)
    {
        return new InquiryFlightDto(
            Date: inquiry.Return.Date,
            SourceAirport: inquiry.Return.SourceAirport,
            TargetAirport: inquiry.Return.TargetAirport);
    }
    

    private static InquiryPassengersDataDto CreatePassengers(Inquiry inquiry)
    {
        var declaredPassengers = new InquiryPassengersDataDto(
            InfantCount: inquiry.DeclaredPassengers.InfantCount,
            ChildrenCount: inquiry.DeclaredPassengers.ChildrenCount,
            AdultCount: inquiry.DeclaredPassengers.AdultCount);
        return declaredPassengers;
    }

    private static IReadOnlyCollection<InquiryPriorityChoiceDto> CreatePriorities(Inquiry inquiry)
    {
        return inquiry.Priorities
            .Select(p => new InquiryPriorityChoiceDto((uint)p.Feature, p.Priority))
            .ToList();
    }
}