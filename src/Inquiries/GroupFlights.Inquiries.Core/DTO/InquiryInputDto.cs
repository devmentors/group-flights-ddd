using GroupFlights.Inquiries.Core.Models;

namespace GroupFlights.Inquiries.Core.DTO;

public record InquiryInputDto(
    Guid? Id,
    InquirerDto Inquirer,
    InquiryFlightDto Travel,
    InquiryFlightDto Return,
    InquiryPassengersDataDto DeclaredPassengers,
    IReadOnlyCollection<InquiryPriorityChoiceDto> Priorities,
    bool CheckedBaggageRequired,
    bool AdditionalServicesRequired,
    string Comments);