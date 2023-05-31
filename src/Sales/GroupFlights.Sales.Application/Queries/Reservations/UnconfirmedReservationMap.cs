using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Variants;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Passengers;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.OfferDraft;

namespace GroupFlights.Sales.Application.Queries.Reservations;

public class UnconfirmedReservationMap
{
    public static ReservationDto Map(UnconfirmedReservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id.Value,
            SourceOfferId = reservation.SourceOfferId,
            Type = nameof(UnconfirmedReservation),
            Status = "Unconfirmed",
            AirlineOfferId = reservation.AirlineOfferId,
            Client = new ClientDto(
                reservation.Client.UserId?.Value.ToString() ?? "", 
                reservation.Client.Name, 
                reservation.Client.Surname,
                reservation.Client.Email.Value,
                reservation.Client.PhoneNumber),
            DeclaredPassengers = new PassengersDataDto(
                InfantCount: reservation.DeclaredPassengers.InfantCount,
                ChildrenCount: reservation.DeclaredPassengers.ChildrenCount,
                AdultCount: reservation.DeclaredPassengers.AdultCount),
            PassengerNamesRequiredImmediately = reservation.PassengerNamesRequiredImmediately,
            PassengerNamesDeadline = new DeadlineDto(
                reservation.PassengerNamesDeadline.DueDate,
                reservation.PassengerNamesDeadline.Fulfilled),
            ProvidedPassengers = (reservation.ProvidedPassengers ?? new List<Passenger>()).Select(Map).ToList(),
            ContractToSign = new ContractToSignDto(
                reservation.ContractToSign.ContractId, 
                reservation.ContractToSign.Signed),
            ContractToSignDeadline = new DeadlineDto(
                reservation.ContractToSignDeadline.DueDate,
                reservation.ContractToSignDeadline.Fulfilled),
            MarkedOverdue = reservation.MarkedOverdue
        };
    }

    private static FlightSegmentDto CreateFlightSegment(FlightSegment flightSegment)
    {
        return new FlightSegmentDto(flightSegment.Date,
            flightSegment.SourceAirport,
            flightSegment.TargetAirport,
            new FlightTimeDto(flightSegment.FlightTime.Hours, flightSegment.FlightTime.Minutes)
        );
    }
    
    private static PassengerDto Map(Passenger passenger)
    {
        var document = new TravelDocumentDto(
            passenger.Document.Type,
            passenger.Document.Number,
            passenger.Document.Series,
            passenger.Document.ExpirationDate);

        return new PassengerDto(passenger.Firstname, passenger.Middlename, passenger.Surname, document);
    }
}