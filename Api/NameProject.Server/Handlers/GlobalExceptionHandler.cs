﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics;

namespace NameProject.Server.Handlers;

[ExcludeFromCodeCoverage]
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private static readonly Dictionary<Type, (int StatusCode, string Message)> ExceptionMap = new()
    {
        { typeof(BadHttpRequestException),        (StatusCodes.Status400BadRequest,    "Invalid request") },
        { typeof(UnauthorizedAccessException),(StatusCodes.Status401Unauthorized,  "Unauthorized") }
        // add your custom exceptions here
    };
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        if (ExceptionMap.TryGetValue(exception.GetType(), out var mapped))
        {
            httpContext.Response.StatusCode = mapped.StatusCode;
            await httpContext.Response.WriteAsync(mapped.Message, cancellationToken);
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync("Internal server error", cancellationToken);
        }

        return await Task.FromResult(true);
    }
}

