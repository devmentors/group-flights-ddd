using GroupFlights.Sales.Domain.Shared.Exceptions;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Shared;

public class PassengersData
{
    private const ushort MinimumAdultCount = 1;

    private PassengersData()
    {
    }
    
    public PassengersData(ushort infantCount, ushort childrenCount, ushort adultCount)
    {
        if (adultCount < MinimumAdultCount)
        {
            throw new MinimumAdultCountNotMetException(expected: MinimumAdultCount, actual: adultCount);
        }
        
        InfantCount = infantCount;
        ChildrenCount = childrenCount;
        AdultCount = adultCount;
        
        var declaredPassengersCount = AdultCount +
                                      ChildrenCount +
                                      InfantCount;
        
        var minimalPassengersCount = 10;
        if (declaredPassengersCount < minimalPassengersCount)
        {
            throw new ThisIsNotAGroupFlightException(minimalPassengersCount, declaredPassengersCount);
        }

        var maxPassengersCount = 1000;
        if (declaredPassengersCount > maxPassengersCount)
        {
            throw new MaxPassengersCountExceededException(1000, declaredPassengersCount);
        }
    }

    public ushort InfantCount { get; init; }
    public ushort ChildrenCount { get; init; }
    public ushort AdultCount { get; init; }
    internal ushort TotalCount => (ushort)(InfantCount + ChildrenCount + AdultCount);
}

internal class MinimumAdultCountNotMetException : HumanPresentableException
{
    internal ushort Expected { get; }
    internal ushort Actual { get; }

    internal MinimumAdultCountNotMetException(ushort expected, ushort actual) 
        : base("Co najmniej jeden dorosły pasażer musi lecieć z grupą!", ExceptionCategory.ValidationError)
    {
        Expected = expected;
        Actual = actual;
    }
}