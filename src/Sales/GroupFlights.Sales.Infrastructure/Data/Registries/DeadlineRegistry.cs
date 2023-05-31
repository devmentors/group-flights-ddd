using GroupFlights.Sales.Application.DeadlineRegistry;
using GroupFlights.Sales.Infrastructure.Data.EF;
using GroupFlights.Shared.Types.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Sales.Infrastructure.Data.Registries;

internal class DeadlineRegistry : IDeadlineRegistry
{
    private readonly SalesDbContext _dbContext;

    public DeadlineRegistry(SalesDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task SaveMapping(DeadlineRegistryEntry deadlineRegistryEntry, CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.DeadlineRegistryEntries
            .AnyAsync(d => d.DeadlineId.Equals(deadlineRegistryEntry.DeadlineId), cancellationToken);

        if (exists)
        {
            throw new AlreadyExistsException();
        }

        await _dbContext.AddAsync(deadlineRegistryEntry, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<DeadlineRegistryEntry> GetByDeadlineId(Guid deadlineId, CancellationToken cancellationToken = default)
    {
        var registryEntry = await _dbContext.DeadlineRegistryEntries
            .SingleOrDefaultAsync(d => d.DeadlineId.Equals(deadlineId), cancellationToken);

        if (registryEntry is null)
        {
            throw new DoesNotExistException();
        }

        return registryEntry;
    }
}