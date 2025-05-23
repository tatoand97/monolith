using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace NameProject.Server.Configs;

public static class OpenTelemetryConfig
{
    /// Configures OpenTelemetry for an application, including metrics and tracing instrumentation,
    /// and sets up exporters for exporting telemetry data.
    /// This method integrates various built-in instrumentation for metrics and tracing, such as runtime,
    /// process, HTTP client, EF Core, gRPC client, and ASP.NET Core.
    /// Additionally, it configures OpenTelemetry exporters using the OTLP endpoint.
    /// <param name="services">
    /// The IServiceCollection to add the OpenTelemetry configuration to.
    /// </param>
    /// <returns>
    /// The updated IServiceCollection with OpenTelemetry configuration added.
    /// </returns>
    public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddBuiltInMetrics();
            })
            .WithTracing(tracing =>
            {
                tracing.AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation();
            });

        services.AddOpenTelemetryExporters();
        
        return services;
    }

    private static IServiceCollection AddOpenTelemetryExporters(this IServiceCollection services)
    {
        var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");

        services.ConfigureOpenTelemetryMeterProvider(meterProvider =>
        {
            meterProvider.AddOtlpExporter(opt =>
            {
                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                    opt.Endpoint = new Uri(otlpEndpoint);
            });
        });

        services.ConfigureOpenTelemetryTracerProvider(tracerProvider =>
        {
            tracerProvider.AddOtlpExporter(opt =>
            {
                if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                    opt.Endpoint = new Uri(otlpEndpoint);
            });
        });

        return services;
    }
    
    private static MeterProviderBuilder AddBuiltInMetrics(this MeterProviderBuilder builder) =>
    builder.AddMeter("Microsoft.AspNetCore.Hosting",
        "Microsoft.AspNetCore.Server.Kestrel",
        "Microsoft.AspNetCore.Http.Connections",
        "Microsoft.AspNetCore.Routing",
        "Microsoft.AspNetCore.Diagnostics",
        "Microsoft.AspNetCore.RateLimiting");
}