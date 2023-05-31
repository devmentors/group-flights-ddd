using GroupFlights.WorkloadManagement.Core.Models;

namespace GroupFlights.WorkloadManagement.Core.Data;

internal interface IWorkloadRepository
{
    Task<ActiveWorkloads> GetActiveWorkloads(CancellationToken cancellationToken);
    Task UpdateWorkloads(ActiveWorkloads workloads, CancellationToken cancellationToken);
}