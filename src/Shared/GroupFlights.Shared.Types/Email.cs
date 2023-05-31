using System.Text.RegularExpressions;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Shared.Types;

public class Email
{
    private static Regex EmailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    
    public string Value { get; private set; }
    
    private Email(){}
    
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        if (EmailRegex.IsMatch(value) is false)
        {
            throw new InvalidEmailFormatException(Value);
        }
        
        Value = value;
    }

    public class InvalidEmailFormatException : HumanPresentableException
    {
        public string AttemptedValue { get; }

        public InvalidEmailFormatException(string attemptedValue) 
            : base($"'{attemptedValue}' nie jest prawidłowym formatem adresu e-mail!", ExceptionCategory.ValidationError)
        {
            AttemptedValue = attemptedValue;
        }
    }
}