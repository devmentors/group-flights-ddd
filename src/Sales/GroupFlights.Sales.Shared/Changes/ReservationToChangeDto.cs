namespace GroupFlights.Sales.Shared.Changes;

public record ReservationToChangeDto(Guid ReservationId,
    AirlineType AirlineType,
    bool IsCompleted,
    ReservationCostDto CurrentCost,
    IReadOnlyCollection<FlightSegmentDto> CurrentTravel,
    IReadOnlyCollection<RequiredPaymentDto> CurrentPayments,
    DeadlineDto PassengerNamesDeadline);