namespace GroupFlights.Sales.Application.PaymentRegistry;

internal record PaymentRegistryEntry(Guid PaymentId, string SourceType, Guid SourceId);