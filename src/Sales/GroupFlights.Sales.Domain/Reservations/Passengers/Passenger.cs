namespace GroupFlights.Sales.Domain.Reservations.Passengers;

public class Passenger
{
    private Passenger(){}
    
    public Passenger(string firstname, string middlename, string surname, TravelDocument document)
    {
        this.Firstname = firstname;
        this.Middlename = middlename;
        this.Surname = surname;
        this.Document = document;
    }

    public string Firstname { get; init; }
    public string Middlename { get; init; }
    public string Surname { get; init; }
    public TravelDocument Document { get; init; }
}