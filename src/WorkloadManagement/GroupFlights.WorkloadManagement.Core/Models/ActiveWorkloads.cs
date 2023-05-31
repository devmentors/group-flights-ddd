using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.WorkloadManagement.Core.Exceptions;
using GroupFlights.WorkloadManagement.Core.Services;

namespace GroupFlights.WorkloadManagement.Core.Models;

internal class ActiveWorkloads
{
    private List<CashierWorkloadAssignment> _workloadAssignments = new();
    private List<WorkloadAssignmentChange> _pendingChanges = new();
    public Guid Id { get; private set; }
    
    private ActiveWorkloads() {}
    
    internal ActiveWorkloads(List<CashierWorkloadAssignment> existingAssignments)
    {
        _workloadAssignments = existingAssignments;
        Id = Guid.NewGuid();
    }

    public IReadOnlyCollection<WorkloadAssignmentChange> PendingChanges => _pendingChanges;

    public void AssignWorkload(CashierId cashierId, string workloadType, string workloadSourceId, IWorkloadConfiguration workloadConfiguration)
    {
        var cashierActiveAssignmentsCount = 0;
        
        
        foreach (var assignment in _workloadAssignments)
        {
            if (assignment.CashierId == cashierId)
            {
                cashierActiveAssignmentsCount++;
            }

            if (AssignmentEqualToInput(cashierId, workloadType, workloadSourceId, assignment))
            {
                throw new AlreadyExistsException();
            }

            if (cashierActiveAssignmentsCount >= workloadConfiguration.CashierAssignmentsLimit)
            {
                throw new WorkloadAssignmentLimitReached(workloadConfiguration);
            }

            if (WorkloadEqual(workloadType, workloadSourceId, assignment))
            {
                throw new WorkloadAlreadyAssignedToOtherCashierException();
            }
        }

        var cashierWorkloadAssignment = new CashierWorkloadAssignment(Guid.NewGuid(), cashierId, workloadType, workloadSourceId);
        _workloadAssignments.Add(cashierWorkloadAssignment);
        _pendingChanges.Add(new (cashierWorkloadAssignment, AssignmentChangeType.Added));
    }

    public void UnassignWorkload(CashierId cashierId, string workloadType, string workloadSourceId)
    {
        var assignments = _workloadAssignments.ToList();
        
        foreach (var assignment in assignments)
        {
            if (AssignmentEqualToInput(cashierId, workloadType, workloadSourceId, assignment))
            {
                _workloadAssignments.Remove(assignment);
                _pendingChanges.Add(new (assignment, AssignmentChangeType.Deleted));
                return;
            }
        }

        throw new DoesNotExistException();
    }

    public bool CanAccessWorkload(string workloadType, string workloadSourceId, CashierId cashierId)
    {
        return _workloadAssignments.Exists(a =>
            AssignmentEqualToInput(cashierId, workloadType, workloadSourceId, a));
    }
    
    private static bool AssignmentEqualToInput(
        CashierId cashierId,
        string workloadType,
        string workloadSourceId,
        CashierWorkloadAssignment existingAssignment)
    {
        return existingAssignment.CashierId == cashierId 
               && existingAssignment.WorkloadType.Equals(workloadType) 
               && existingAssignment.WorkloadSourceId.Equals(workloadSourceId);
    }

    private static bool WorkloadEqual(
        string workloadType, 
        string workloadSourceId,
        CashierWorkloadAssignment existingAssignment)
    {
        return existingAssignment.WorkloadType.Equals(workloadType)
            && existingAssignment.WorkloadSourceId.Equals(workloadSourceId);
    }
}