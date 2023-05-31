using GroupFlights.Postsale.Shared.IntegrationEvents;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Operations;

namespace GroupFlights.Postsale.Application.EventHandlers.Internal;

public class ReservationChangeAcceptedIntegrationEventHandler : IEventHandler<ReservationChangeAcceptedIntegrationEvent>
{
    private readonly ISalesApi _salesApi;
    private readonly ITimeManagementApi _timeManagementApi;

    public ReservationChangeAcceptedIntegrationEventHandler(ISalesApi salesApi, ITimeManagementApi timeManagementApi)
    {
        _salesApi = salesApi ?? throw new ArgumentNullException(nameof(salesApi));
        _timeManagementApi = timeManagementApi ?? throw new ArgumentNullException(nameof(timeManagementApi));
    }
    
    public async Task HandleAsync(ReservationChangeAcceptedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var deadlinesToModify = @event.PaymentDeadlineChanges
            .Select(p => new { p.DeadlineId, p.NewDueDate }).Concat(new[]
            { new { @event.PassengerNamesDeadlineChange.DeadlineId, @event.PassengerNamesDeadlineChange.NewDueDate } });
        
        foreach (var deadlineChange in deadlinesToModify)
        {
            await _timeManagementApi.UpdateDeadline(new UpdateDeadlineDueDateDto(
                new DeadlineId(deadlineChange.DeadlineId), deadlineChange.NewDueDate));
        }

        await _salesApi.ChangeReservation(new ChangeReservationCommand(
            @event.ReservationId,
            @event.ReservationChangeRequestId,
            @event.ChangeTravel,
            @event.CostAfterChange,
            @event.PaymentDeadlineChanges,
            @event.PassengerNamesDeadlineChange),
        cancellationToken);
    }
}