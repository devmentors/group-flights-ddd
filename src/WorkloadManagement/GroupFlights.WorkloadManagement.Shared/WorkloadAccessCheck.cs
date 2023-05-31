using GroupFlights.Shared.Types;

namespace GroupFlights.WorkloadManagement.Shared;

public record WorkloadAccessCheck(string WorkloadType, string WorkloadSourceId, CashierId CashierId);