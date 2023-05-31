using GroupFlights.WorkloadManagement.Core.Services;

namespace GroupFlights.WorkloadManagement.Api;

internal class WorkloadConfiguration : IWorkloadConfiguration
{
    public WorkloadConfiguration(ushort maxAssignmentsAtOnce)
    {
        if (maxAssignmentsAtOnce <= 1) throw new ArgumentOutOfRangeException(nameof(maxAssignmentsAtOnce));
        CashierAssignmentsLimit = maxAssignmentsAtOnce;
    }

    public ushort CashierAssignmentsLimit { get; }
}