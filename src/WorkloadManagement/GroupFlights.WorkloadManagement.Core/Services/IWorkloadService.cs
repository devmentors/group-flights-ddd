using GroupFlights.Shared.Types;
using GroupFlights.WorkloadManagement.Core.DTO;
using GroupFlights.WorkloadManagement.Shared;

namespace GroupFlights.WorkloadManagement.Core.Services;

public interface IWorkloadService
{
    Task<bool> CanAccessWorkload(WorkloadAccessCheck request, CancellationToken cancellationToken = default);
    Task AssignCashierToWorkload(AssignCashierToWorkload request, CancellationToken cancellationToken = default);
    Task UnassignCashierFromWorkload(UnassignCashierFromWorkload request, CancellationToken cancellationToken = default);
}