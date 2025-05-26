namespace NameProject.Server.Middlewares;

public class AddHeadersMiddleware(RequestDelegate requestDelegate)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("Strict-Transport-Security", "max-age=86400; includeSubDomains");
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self' *.tuya.com.co; script-src 'self' *.tuya.com.co; style-src 'self' 'unsafe-inline' *.tuya.com.co; img-src 'self' *.tuya.com.co");
            context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=(), fullscreen=(self), payment()");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin");
            context.Response.Headers.Append("Cache-Control", "'no-cache', 'no-store', 'must-revalidate', 'max-age=0', 's-maxage=0'");
            context.Response.Headers.Append("Pragma", "no-cache");
            context.Response.Headers.Append("Expires", "0");
            context.Response.Headers.Append("Content-Disposition", "filename");
            context.Response.Headers.Append("X-Download-Options", "noopen");
            
            return Task.CompletedTask;
        });
        await requestDelegate(context);
    }
}