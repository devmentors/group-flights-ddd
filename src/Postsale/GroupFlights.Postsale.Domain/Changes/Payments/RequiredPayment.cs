using GroupFlights.Postsale.Domain.Shared;

namespace GroupFlights.Postsale.Domain.Changes.Payments;

public record RequiredPayment(Guid PaymentId, Deadline Deadline, bool Payed = false)
{
    private RequiredPayment() : this(default, default, default)
    {
    }
}