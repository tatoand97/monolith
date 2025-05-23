using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Serilog.Context;

namespace NameProject.Server.Middlewares;

[ExcludeFromCodeCoverage]
internal sealed class LogContextTraceMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

        using (LogContext.PushProperty("TraceId", traceId))
        {
            return next(context);
        }
    }
}