using GroupFlights.Shared.Types;
using GroupFlights.WorkloadManagement.Core.Services;
using GroupFlights.WorkloadManagement.Shared;

namespace GroupFlights.WorkloadManagement.Core.ModuleApi;

public class WorkloadManagementApi : IWorkloadManagementApi
{
    private readonly IWorkloadService _workloadService;

    public WorkloadManagementApi(IWorkloadService workloadService)
    {
        _workloadService = workloadService ?? throw new ArgumentNullException(nameof(workloadService));
    }
    
    public async Task<bool> CanAccessWorkload(WorkloadAccessCheck request, CancellationToken cancellationToken = default)
    {
        return await _workloadService.CanAccessWorkload(request, cancellationToken);
    }
}