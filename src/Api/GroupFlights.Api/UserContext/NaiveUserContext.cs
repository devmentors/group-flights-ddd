using GroupFlights.Shared.Plumbing.UserContext;
using GroupFlights.Shared.Types;

namespace GroupFlights.Api.UserContext;

internal class NaiveUserContext : IUserContext
{
    private const string UserIdHeader = "X-UserId";
    private const string CashierIdHeader = "X-CashierId";
    private const string AdminHeader = "X-IsAdmin";
    
    private NaiveUserContext()
    {
        
    }
    
    public UserId UserId { get; private set; }
    public CashierId CashierId { get; private set; }
    public bool IsCashier => UserId is not null && CashierId is not null;
    public bool IsClient => UserId is not null;
    public bool IsAdministrator { get; init; }

    public static NaiveUserContext CreateFrom(HttpContext httpContext, ILogger logger)
    {
        var userIdFound = httpContext.Request.Headers.TryGetValue(UserIdHeader, out var userId);
        var cashierIdFound = httpContext.Request.Headers.TryGetValue(CashierIdHeader, out var cashierId);
        var isAdminFound = httpContext.Request.Headers.TryGetValue(AdminHeader, out var isAdmin);
        
        try
        {
            return new NaiveUserContext
            {
                UserId = userIdFound ? new UserId(Guid.Parse(userId.FirstOrDefault() ?? string.Empty)) : null,
                CashierId = cashierIdFound ? new CashierId(Guid.Parse(cashierId.FirstOrDefault() ?? string.Empty)) : null,
                IsAdministrator = isAdminFound
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new NaiveUserContext();
        }
    }
}