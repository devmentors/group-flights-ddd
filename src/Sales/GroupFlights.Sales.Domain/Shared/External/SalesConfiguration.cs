namespace GroupFlights.Sales.Domain.Shared.External;

public record SalesConfiguration(TimeSpan DefaultOfferValidTime, TimeSpan ContractSignTime);