namespace GroupFlights.Sales.Application.DeadlineRegistry;

public record DeadlineRegistryEntry(Guid DeadlineId, string SourceType, Guid SourceId)
{
    private DeadlineRegistryEntry() : this(default, default, default){}
}