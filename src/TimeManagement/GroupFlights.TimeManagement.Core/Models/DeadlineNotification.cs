namespace GroupFlights.TimeManagement.Core.Models;

internal class DeadlineNotification
{
    private DeadlineNotification(){}
    
    public DeadlineNotification(DateTime dueDate, CompletionState state = CompletionState.Pending)
    {
        DueDate = dueDate;
        State = state;
    }

    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime DueDate { get; init; }
    public CompletionState State { get; internal set; }
    

    public enum CompletionState
    {
        Pending,
        Sent,
        Canceled
    }
}