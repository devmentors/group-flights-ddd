using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.TimeManagement.Shared;

public interface ITimeManagementApi
{
    Task SetUpDeadline(SetUpDeadlineDto request, CancellationToken cancellationToken = default);
    Task MarkDeadlineFulfilled(DeadlineId deadlineId, CancellationToken cancellationToken = default);
    Task UpdateDeadline(UpdateDeadlineDueDateDto dueDateUpdate, CancellationToken cancellationToken = default);
}