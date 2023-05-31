using GroupFlights.Shared.Types;

namespace GroupFlights.WorkloadManagement.Core.DTO;

public record UnassignCashierFromWorkload(string WorkloadType, string WorkloadSourceId);