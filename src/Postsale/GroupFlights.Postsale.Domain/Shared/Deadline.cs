namespace GroupFlights.Postsale.Domain.Shared;

public record Deadline(Guid Id, DateTime DueDate, bool? Fulfilled = null)
{
    private Deadline() : this(default, default, default)
    {
    }
}