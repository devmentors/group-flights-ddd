using GroupFlights.Shared.Plumbing.UserContext;
using GroupFlights.WorkloadManagement.Core.Data;
using GroupFlights.WorkloadManagement.Core.DTO;
using GroupFlights.WorkloadManagement.Shared;

namespace GroupFlights.WorkloadManagement.Core.Services;

internal class WorkloadService : IWorkloadService
{
    private readonly IWorkloadRepository _repository;
    private readonly IWorkloadConfiguration _workloadConfiguration;
    private readonly IUserContextAccessor _userContextAccessor;

    public WorkloadService(IWorkloadRepository repository, IWorkloadConfiguration workloadConfiguration, IUserContextAccessor userContextAccessor)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _workloadConfiguration = workloadConfiguration ?? throw new ArgumentNullException(nameof(workloadConfiguration));
        _userContextAccessor = userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));
    }
    
    public async Task<bool> CanAccessWorkload(WorkloadAccessCheck request, CancellationToken cancellationToken = default)
    {
        var activeWorkloads = await _repository.GetActiveWorkloads(cancellationToken);
        return activeWorkloads.CanAccessWorkload(
            request.WorkloadType, 
            request.WorkloadSourceId,
            _userContextAccessor.Get().CashierId);
    }
    public async Task AssignCashierToWorkload(AssignCashierToWorkload request, CancellationToken cancellationToken = default)
    {
        var activeWorkloads = await _repository.GetActiveWorkloads(cancellationToken);
        activeWorkloads.AssignWorkload(
            _userContextAccessor.Get().CashierId,
            request.WorkloadType, 
            request.WorkloadSourceId,
            _workloadConfiguration);

        await _repository.UpdateWorkloads(activeWorkloads, cancellationToken);
    }

    public async Task UnassignCashierFromWorkload(UnassignCashierFromWorkload request, CancellationToken cancellationToken = default)
    {
        var activeWorkloads = await _repository.GetActiveWorkloads(cancellationToken);
        activeWorkloads.UnassignWorkload(
            _userContextAccessor.Get().CashierId,
            request.WorkloadType, 
            request.WorkloadSourceId);

        await _repository.UpdateWorkloads(activeWorkloads, cancellationToken);
        
    }
}