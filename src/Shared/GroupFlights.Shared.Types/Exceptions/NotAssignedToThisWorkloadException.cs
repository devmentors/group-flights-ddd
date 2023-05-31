namespace GroupFlights.Shared.Types.Exceptions;

public class NotAssignedToThisWorkloadException : HumanPresentableException
{
    public NotAssignedToThisWorkloadException() 
        : base("Nie jesteś przypisywany do tego obiektu. Przypnij się do niego jeśli chcesz kontynuuować.", 
            ExceptionCategory.ValidationError)
    {
    }
}