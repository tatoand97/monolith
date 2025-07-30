using System.Diagnostics.CodeAnalysis;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace NameProject.Server.Handlers;

[ExcludeFromCodeCoverage]
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        switch (exception)
        {
            case ModelValidationException validationEx:
                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    message = validationEx.Message,
                    errors = validationEx.Errors
                }, cancellationToken);
                break;

            case UnauthorizedAccessException:
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Unauthorized", cancellationToken);
                break;

            default:
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Internal server error", cancellationToken);
                break;
        }

        return await Task.FromResult(true);
    }
}

