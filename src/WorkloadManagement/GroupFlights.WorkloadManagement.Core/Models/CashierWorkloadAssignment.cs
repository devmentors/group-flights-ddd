using GroupFlights.Shared.Types;

namespace GroupFlights.WorkloadManagement.Core.Models;

internal record CashierWorkloadAssignment(Guid AssignmentId, CashierId CashierId, string WorkloadType, string WorkloadSourceId)
{
    private CashierWorkloadAssignment(): this(default, default, default, default)
    {
    }
}