namespace GroupFlights.Postsale.Domain.Changes.Outcome;

public record PaymentDeadlineChange(Guid PaymentId, DateTime NewDueDate, Guid DeadlineId)
{
    private PaymentDeadlineChange() : this(default, default, default)
    {
        
    }
}