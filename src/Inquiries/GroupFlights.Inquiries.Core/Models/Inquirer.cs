using GroupFlights.Shared.Types;

namespace GroupFlights.Inquiries.Core.Models;

internal record Inquirer(UserId UserId, string Name, string Surname, Email Email, string PhoneNumber)
{
    private Inquirer() : this(default, default, default, default, default)
    {
        
    }
}