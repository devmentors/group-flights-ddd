namespace GroupFlights.Sales.Domain.Reservations.Passengers;

public record TravelDocument(string Type, string Number, string Series, DateTime ExpirationDate);