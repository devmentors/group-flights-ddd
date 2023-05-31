using GroupFlights.Shared.Types;

namespace GroupFlights.WorkloadManagement.Shared;

public interface IWorkloadManagementApi
{
    Task<bool> CanAccessWorkload(WorkloadAccessCheck request, CancellationToken cancellationToken = default);
}