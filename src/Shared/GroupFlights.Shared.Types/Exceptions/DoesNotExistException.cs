namespace GroupFlights.Shared.Types.Exceptions;

public class DoesNotExistException : HumanPresentableException
{
    public DoesNotExistException(string objectName = null) 
        : base("Szukany obiekt" + (objectName != null ? $" {objectName}": "") +
               " nie istnieje w systemie!", ExceptionCategory.NotFound)
    {
    }
}