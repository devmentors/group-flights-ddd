namespace GroupFlights.Postsale.Domain.Changes.Outcome;

public record PassengerNamesDeadlineChange(Guid DeadlineId, DateTime NewDueDate)
{
    private PassengerNamesDeadlineChange() : this(default, default)
    {
        
    }
}