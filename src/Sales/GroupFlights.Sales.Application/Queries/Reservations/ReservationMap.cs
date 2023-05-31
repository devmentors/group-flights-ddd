using GroupFlights.Sales.Application.DTO;
using GroupFlights.Sales.Domain.Reservations;
using GroupFlights.Sales.Domain.Reservations.Passengers;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.OfferDraft;

namespace GroupFlights.Sales.Application.Queries.Reservations;

internal static class ReservationMap
{
    public static ReservationDto Map(Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id.Value,
            SourceOfferId = reservation.SourceOfferId,
            Type = nameof(Reservation),
            Status = reservation.Status?.ToString(),
            AirlineOfferId = reservation.AirlineOfferId,
            Client = new ClientDto(
                reservation.Client.UserId?.Value.ToString() ?? "", 
                reservation.Client.Name, 
                reservation.Client.Surname,
                reservation.Client.Email.Value,
                reservation.Client.PhoneNumber),
            AirlineType = reservation.AirlineType,
            AirlineName = reservation.AirlineName,
            Travel = reservation.Travel.Select(CreateFlightSegment).ToList(),
            Return = reservation.Return.Select(CreateFlightSegment).ToList(),
            DeclaredPassengers = new PassengersDataDto(
                InfantCount: reservation.DeclaredPassengers.InfantCount,
                ChildrenCount: reservation.DeclaredPassengers.ChildrenCount,
                AdultCount: reservation.DeclaredPassengers.AdultCount),
            PassengerNamesRequiredImmediately = reservation.PassengerNamesRequiredImmediately,
            PassengerNamesDeadline = new DeadlineDto(
                reservation.PassengerNamesDeadline.DueDate,
                reservation.PassengerNamesDeadline.Fulfilled),
            ProvidedPassengers = (reservation.ProvidedPassengers ?? new List<Passenger>()).Select(Map).ToList(),
            Cost = reservation.Cost.TotalCost,
            CanChangePassengerNames = reservation.CanChangePassengersOrTravel,
            RequiredPayments = reservation.RequiredPayments?.Select(rp => 
                    new RequiredPaymentDto(
                        rp.PaymentId, 
                        new DeadlineDto(rp.Deadline.DueDate, rp.Deadline.Fulfilled)))
                .ToList()
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