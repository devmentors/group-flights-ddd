using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Api.ErrorHandling;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private const string SomethingWentWrong = "Coś poszło nie tak!";

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await WriteErrorToResponse(context, ex);
        }
    }

    private async Task WriteErrorToResponse(HttpContext context, Exception ex)
    {
        if (ex is HumanPresentableException humanReadable)
        {
            context.Response.StatusCode = (int)humanReadable.ExceptionCategory.AsStatusCode();
            await context.Response.WriteAsJsonAsync(new ErrorPayload(humanReadable.Message));
            return;
        }

        _logger.LogError(ex, ex.Message);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new ErrorPayload(SomethingWentWrong));
    }
}