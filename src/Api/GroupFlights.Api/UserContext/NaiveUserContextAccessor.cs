using GroupFlights.Shared.Plumbing.UserContext;

namespace GroupFlights.Api.UserContext;

public class NaiveUserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<NaiveUserContextAccessor> _logger;

    public NaiveUserContextAccessor(IHttpContextAccessor contextAccessor, ILogger<NaiveUserContextAccessor> logger)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public IUserContext Get()
    {
        return NaiveUserContext.CreateFrom(_contextAccessor.HttpContext, _logger);
    }
}