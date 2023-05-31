using GroupFlights.Api.ErrorHandling;
using GroupFlights.Shared.ModuleDefinition;
using GroupFlights.Shared.Plumbing.UserContext;

namespace GroupFlights.Api.UserContext;

internal class NaiveAccessControlMiddleware : IMiddleware
{
    private readonly ILogger<NaiveAccessControlMiddleware> _logger;

    public static readonly ErrorPayload ForbiddenErrorPayload =
        new ErrorPayload("Nie masz uprawnień do wykonanania tej operacji!"); 

    public NaiveAccessControlMiddleware(ILogger<NaiveAccessControlMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var routePattern = (context.GetEndpoint() as RouteEndpoint)?.RoutePattern;
        Enum.TryParse<HttpVerb>(context.Request.Method, out var httpVerb);
        var routeKey = new RegistrationRouteKey(routePattern?.RawText, httpVerb);
        
        var requiredAccess = Modules.ResolveAccessFor(routeKey);

        if (requiredAccess == NaiveAccessControl.Anonymous)
        {
            await next(context);
            return;
        }
        
        var userContext = NaiveUserContext.CreateFrom(context, _logger);

        if (!IsAuthorized(userContext, requiredAccess))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(ForbiddenErrorPayload);
            return;
        }

        await next(context);
    }

    private bool IsAuthorized(IUserContext userContext, NaiveAccessControl requiredAccess)
    {
        switch (requiredAccess)
        {
            case NaiveAccessControl.CashierOnly:
                return userContext.IsCashier;
            case NaiveAccessControl.ClientOnly:
                return userContext.IsClient;
            case NaiveAccessControl.ClientAndCashier:
                return userContext.IsCashier || userContext.IsClient;
            default:
                throw new NotSupportedException($"{nameof(NaiveAccessControl)}='{requiredAccess.ToString()}'");
        }
    }
}