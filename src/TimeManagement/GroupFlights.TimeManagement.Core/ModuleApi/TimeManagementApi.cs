using GroupFlights.TimeManagement.Core.Services;
using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.TimeManagement.Core.ModuleApi;

internal class TimeManagementApi : ITimeManagementApi
{
    private readonly IDeadlineService _deadlineService;

    public TimeManagementApi(IDeadlineService deadlineService)
    {
        _deadlineService = deadlineService ?? throw new ArgumentNullException(nameof(deadlineService));
    }
    
    public async Task SetUpDeadline(SetUpDeadlineDto request, CancellationToken cancellationToken = default)
    {
        await _deadlineService.SetUpDeadline(request, cancellationToken);
    }

    public async Task MarkDeadlineFulfilled(DeadlineId deadlineId, CancellationToken cancellationToken = default)
    {
        await _deadlineService.MarkDeadlineFulfilled(deadlineId, cancellationToken);
    }

    public async Task UpdateDeadline(UpdateDeadlineDueDateDto dueDateUpdate, CancellationToken cancellationToken = default)
    {
        await _deadlineService.UpdateDeadline(dueDateUpdate, cancellationToken);
    }
}