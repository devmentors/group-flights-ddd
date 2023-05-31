namespace GroupFlights.Sales.Application.DTO;

public record TravelDocumentDto(string Type, string Number, string Series, DateTime ExpirationDate);