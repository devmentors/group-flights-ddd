using GroupFlights.Inquiries.Core.DTO;
using GroupFlights.Inquiries.Core.Models;
using GroupFlights.Shared.Types;

namespace GroupFlights.Inquiries.Core.Mapping;

internal static class InquiryDtoToInquiry
{
    public static Inquiry Map(InquiryInputDto request)
    {
        return new Inquiry(
            new InquiryId(request.Id ?? Guid.NewGuid()),
            CreateInquirer(request),
            CreateTravel(request),
            CreateReturn(request),
            CreatePassengers(request),
            CreatePriorities(request),
            request.CheckedBaggageRequired,
            request.AdditionalServicesRequired,
            request.Comments);
    }
    
    private static Inquirer CreateInquirer(InquiryInputDto request)
    {
        return new Inquirer(
            UserId: string.IsNullOrEmpty(request.Inquirer.UserId) ? null : new UserId(Guid.Parse(request.Inquirer.UserId)),
            Name: request.Inquirer.Name,
            Surname: request.Inquirer.Surname,
            Email: new Email(request.Inquirer.Email),
            PhoneNumber: request.Inquirer.PhoneNumber);
    }

    private static InquiredFlight CreateTravel(InquiryInputDto request)
    {
        return new InquiredFlight(
            Date: request.Travel.Date,
            SourceAirport: request.Travel.SourceAirport,
            TargetAirport: request.Travel.TargetAirport);
    }
    private static InquiredFlight CreateReturn(InquiryInputDto request)
    {
        return new InquiredFlight(
            Date: request.Return.Date,
            SourceAirport: request.Return.SourceAirport,
            TargetAirport: request.Return.TargetAirport);
    }
    

    private static PassengersData CreatePassengers(InquiryInputDto request)
    {
        var declaredPassengers = new PassengersData(
            infantCount: request.DeclaredPassengers.InfantCount,
            childrenCount: request.DeclaredPassengers.ChildrenCount,
            adultCount: request.DeclaredPassengers.AdultCount);
        return declaredPassengers;
    }

    private static IReadOnlyCollection<PriorityChoice> CreatePriorities(InquiryInputDto request)
    {
        return request.Priorities
            .Select(p => PriorityChoice.Parse(feature: p.Feature, priority: p.Priority))
            .ToList();
    }
}