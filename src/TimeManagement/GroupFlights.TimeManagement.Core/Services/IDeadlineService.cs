using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.TimeManagement.Core.Services;

internal interface IDeadlineService
{
    Task SetUpDeadline(SetUpDeadlineDto request, CancellationToken cancellationToken = default);
    Task MarkDeadlineFulfilled(DeadlineId deadlineId, CancellationToken cancellationToken = default);
    Task ProcessActiveDeadlines(CancellationToken cancellationToken = default);
    Task UpdateDeadline(UpdateDeadlineDueDateDto dueDateUpdate, CancellationToken cancellationToken = default);
}