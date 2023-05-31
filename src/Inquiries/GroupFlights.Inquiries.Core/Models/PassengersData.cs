namespace GroupFlights.Inquiries.Core.Models;

internal class PassengersData
{
    private PassengersData()
    {
    }
    
    internal PassengersData(ushort infantCount, ushort childrenCount, ushort adultCount)
    {
        if (adultCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(adultCount));
        }
        
        InfantCount = infantCount;
        ChildrenCount = childrenCount;
        AdultCount = adultCount;
    }

    public ushort InfantCount { get; init; }
    public ushort ChildrenCount { get; init; }
    public ushort AdultCount { get; init; }
}