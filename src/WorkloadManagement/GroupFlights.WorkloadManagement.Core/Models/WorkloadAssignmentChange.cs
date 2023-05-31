namespace GroupFlights.WorkloadManagement.Core.Models;

internal record WorkloadAssignmentChange(CashierWorkloadAssignment Assignment, AssignmentChangeType ChangeType);

internal enum AssignmentChangeType
{
    Added,
    Deleted
}