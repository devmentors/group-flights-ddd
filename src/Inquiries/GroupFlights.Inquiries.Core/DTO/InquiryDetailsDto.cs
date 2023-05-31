using GroupFlights.Inquiries.Core.Models;

namespace GroupFlights.Inquiries.Core.DTO;

public record InquiryDetailsDto(
    string Id,
    InquirerDto Inquirer,
    InquiryFlightDto Travel,
    InquiryFlightDto Return,
    InquiryPassengersDataDto DeclaredInquiryPassengers,
    IReadOnlyCollection<InquiryPriorityChoiceDto> Priorities,
    bool CheckedBaggageRequired,
    bool AdditionalServicesRequired,
    string Comments,
    InquiryVerificationResult? VerificationResult,
    string RejectionReason);