namespace GroupFlights.Shared.Types.Exceptions;

public abstract class HumanPresentableException : Exception
{
    public ExceptionCategory ExceptionCategory { get; }

    public HumanPresentableException(string message, ExceptionCategory exceptionCategory) : base(message)
    {
        ExceptionCategory = exceptionCategory;
    }
}

public enum ExceptionCategory
{
    ValidationError,
    NotFound,
    AlreadyExists,
    ConcurrencyError,
    TechnicalError
}