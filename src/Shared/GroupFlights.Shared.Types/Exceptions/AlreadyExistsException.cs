namespace GroupFlights.Shared.Types.Exceptions;

public class AlreadyExistsException : HumanPresentableException
{
    public AlreadyExistsException() 
        : base("Szukany obiekt już istnieje w systemie!", ExceptionCategory.AlreadyExists)
    {
    }
}