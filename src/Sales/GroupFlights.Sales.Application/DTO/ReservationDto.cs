using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.OfferDraft;
using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Application.DTO;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid SourceOfferId { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string AirlineOfferId { get; set; }
    public ClientDto Client { get; set; }
    public AirlineType AirlineType { get; set; }
    public string AirlineName { get; set; }
    public List<FlightSegmentDto> Travel { get; set; }
    public List<FlightSegmentDto> Return { get; set; }
    public PassengersDataDto DeclaredPassengers { get; set; }
    public bool PassengerNamesRequiredImmediately { get; init; }
    public DeadlineDto PassengerNamesDeadline { get; set; }
    public List<PassengerDto> ProvidedPassengers { get; set; }
    public Money Cost { get; init; }
    public ContractToSignDto ContractToSign { get; set; }
    public DeadlineDto ContractToSignDeadline { get; set; }
    public bool MarkedOverdue { get; set; }
    public bool CanChangePassengerNames { get; set; }
    public List<RequiredPaymentDto> RequiredPayments { get; set; }
}