using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Inquiries.Core.Exceptions;

public class InvalidInputException : HumanPresentableException
{
    public InvalidInputException(string inputName) 
        : base($"Wprowadzono nieprawidlowa wartosc pola: {inputName}", ExceptionCategory.ValidationError)
    {
    }
}