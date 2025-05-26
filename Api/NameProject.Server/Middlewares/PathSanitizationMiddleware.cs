using NameProject.Server.Utils;

namespace NameProject.Server.Middlewares;

public class PathSanitizationMiddleware
(
    RequestDelegate next,
    ILogger<PathSanitizationMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var originalPath = context.Request.Path.Value ?? "";
        logger.LogInformation("Request Path: {OriginalPath}", originalPath);
        if (PathSanitization.IsMalicius(originalPath))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid Request Path");
            return;
        }
        await next(context);
    }
}