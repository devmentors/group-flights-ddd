using System.ComponentModel;

namespace GroupFlights.Sales.Domain.Offers.Shared;

public class PriorityChoice
{
    public PrioritizedFeature Feature { get; }
    public ushort Priority { get; }

    private PriorityChoice()
    {
        
    }
    
    public PriorityChoice(PrioritizedFeature feature, ushort priority)
    {
        if (!Enum.IsDefined(typeof(PrioritizedFeature), feature))
            throw new InvalidEnumArgumentException(nameof(feature), (int)feature, typeof(PrioritizedFeature));
        Feature = feature;
        Priority = priority;
    }

    public static PriorityChoice Parse(uint feature, ushort priority)
    {
        var validFeatureChoice = Enum.IsDefined(typeof(PrioritizedFeature), feature);
        if (validFeatureChoice is false)
        {
            throw new InvalidPrioritizedFeatureProvidedException(feature);
        }
        return new PriorityChoice((PrioritizedFeature) feature, priority);
    }
    
    public enum PrioritizedFeature : uint
    {
        Price = 1,
        Date = 2,
        Flexibility = 3
    }
}

internal class InvalidPrioritizedFeatureProvidedException : Exception
{
    internal uint InvalidFeatureInteger { get; }

    internal InvalidPrioritizedFeatureProvidedException(uint invalidFeatureInteger)
    {
        InvalidFeatureInteger = invalidFeatureInteger;
    }
}