using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Inquiries.Core.Exceptions;

public class InvalidAirportException : HumanPresentableException
{
    public InvalidAirportException(string airportCodeFromInput) 
        : base($"Podany port lotniczy o kodzie {airportCodeFromInput} nie jest prawidlowy", 
            ExceptionCategory.ValidationError)
    {
    }
}