using GroupFlights.Shared.Types;

namespace GroupFlights.Sales.Domain.Shared;

public class Client
{
    public UserId UserId { get; internal set; }
    public string Name { get; }
    public string Surname { get; }
    public Email Email { get; }
    public string PhoneNumber { get; }

    private Client()
    {
        
    }
    
    public Client(string name, string surname, Email email, string phoneNumber, UserId userId = default)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Surname = surname ?? throw new ArgumentNullException(nameof(surname));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        UserId = userId;
    }
}