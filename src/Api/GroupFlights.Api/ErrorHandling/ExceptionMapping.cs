using System.Net;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Api.ErrorHandling;

internal static class ExceptionMapping
{
    public static HttpStatusCode AsStatusCode(this ExceptionCategory exceptionCategory)
    {
        return exceptionCategory switch
        {
            ExceptionCategory.ValidationError => HttpStatusCode.UnprocessableEntity,
            ExceptionCategory.NotFound => HttpStatusCode.NotFound,
            ExceptionCategory.ConcurrencyError => HttpStatusCode.Conflict,
            ExceptionCategory.TechnicalError => HttpStatusCode.InternalServerError,
            ExceptionCategory.AlreadyExists => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };
    }
}