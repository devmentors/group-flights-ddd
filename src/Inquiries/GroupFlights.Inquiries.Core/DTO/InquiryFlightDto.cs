using GroupFlights.Shared.Types;

namespace GroupFlights.Inquiries.Core.DTO;

public record InquiryFlightDto(DateTime Date, Airport SourceAirport, Airport TargetAirport);