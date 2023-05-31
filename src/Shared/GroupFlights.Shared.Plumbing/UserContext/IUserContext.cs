using GroupFlights.Shared.Types;

namespace GroupFlights.Shared.Plumbing.UserContext;

public interface IUserContext
{
    UserId UserId { get; }
    CashierId CashierId { get; }

    bool IsCashier { get; }
    bool IsClient { get; }
    bool IsAdministrator { get; }
}