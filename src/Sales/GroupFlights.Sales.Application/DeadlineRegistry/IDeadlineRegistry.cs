namespace GroupFlights.Sales.Application.DeadlineRegistry;

public interface IDeadlineRegistry
{
    Task SaveMapping(DeadlineRegistryEntry deadlineRegistryEntry, CancellationToken cancellationToken = default);
    Task<DeadlineRegistryEntry> GetByDeadlineId(Guid deadlineId, CancellationToken cancellationToken = default);
}