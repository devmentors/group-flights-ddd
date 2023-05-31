namespace GroupFlights.Sales.Domain.Reservations.Contract;

public record ContractToSign(Guid ContractId, bool? Signed = default);