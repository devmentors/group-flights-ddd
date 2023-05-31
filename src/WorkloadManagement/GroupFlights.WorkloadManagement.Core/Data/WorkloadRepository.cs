using GroupFlights.WorkloadManagement.Core.Data.EF;
using GroupFlights.WorkloadManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.WorkloadManagement.Core.Data;

internal class WorkloadRepository : IWorkloadRepository
{
    private readonly WorkloadsDbContext _dbContext;

    public WorkloadRepository(WorkloadsDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task<ActiveWorkloads> GetActiveWorkloads(CancellationToken cancellationToken)
    {
        return new ActiveWorkloads(await _dbContext.Workloads.ToListAsync(cancellationToken));
    }

    public async Task UpdateWorkloads(ActiveWorkloads workloads, CancellationToken cancellationToken)
    {
        foreach (var changes in workloads.PendingChanges)
        {
            switch(changes.ChangeType)
            {
                case AssignmentChangeType.Added:
                    await _dbContext.Workloads.AddAsync(changes.Assignment, cancellationToken);
                    break;
                case AssignmentChangeType.Deleted:
                    _dbContext.Remove(changes.Assignment);
                    break;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}