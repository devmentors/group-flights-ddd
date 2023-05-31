namespace GroupFlights.Inquiries.Core.Models;

internal record PriorityChoice(PriorityChoice.PrioritizedFeature Feature, ushort Priority)
{
    private PriorityChoice() : this(default, default)
    {
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
    
    internal enum PrioritizedFeature : uint
    {
        Price = 1,
        Date = 2,
        Flexibility = 3
    }
}

public class InvalidPrioritizedFeatureProvidedException : Exception
{
    public uint InvalidFeatureInteger { get; }

    public InvalidPrioritizedFeatureProvidedException(uint invalidFeatureInteger)
    {
        InvalidFeatureInteger = invalidFeatureInteger;
    }
}