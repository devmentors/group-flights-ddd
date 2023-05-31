namespace GroupFlights.Finance.Core.Models;

public record Payer(Guid PayerId,
    Guid UserId,
    string PayerFullName,
    bool IsLegalEntity,
    string TaxNumber,
    string Address)
{
    private Payer() : this(default, default, default, default, default, default)
    {}
}