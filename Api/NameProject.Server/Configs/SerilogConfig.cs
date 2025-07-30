using NameProject.Server.Utils;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

namespace NameProject.Server.Configs;

/// <summary>
/// Provides extension methods for configuring Serilog in the application.
/// </summary>
public static class SerilogConfig
{
    /// <summary>
    /// Configures Serilog logging for the application and integrates it with the specified host builder.
    /// </summary>
    /// <param name="hostBuilder">The IHostBuilder instance to configure with Serilog and application-specific logging settings.</param>
    /// <returns>The updated IHostBuilder with Serilog logging integrated.</returns>
    public static void UseSerilogCustom(this IHostBuilder hostBuilder)
    {
        AddLoggingSerilog();
        hostBuilder.UseSerilog();
    }
    
    private static void AddLoggingSerilog()
    {
        var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? string.Empty; //Service name
        var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? string.Empty;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.With<ActivityEnricher>()
            .Enrich.WithSensitiveDataMasking(options =>
            {
                options.MaskingOperators =
                [
                    new EmailAddressMaskingOperator(),
                    new CreditCardMaskingOperator(),
                    new IbanMaskingOperator()
                ];
                options.Mode = MaskingMode.Globally;
                options.MaskValue = "*******";
                
            })
            .Enrich.WithProperty("service.name", serviceName)
            .WriteTo.Console()
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = otlpEndpoint;
                options.Protocol = OtlpProtocol.HttpProtobuf;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = serviceName
                };
                options.RestrictedToMinimumLevel = LogEventLevel.Warning;
            })
            .CreateLogger();
    }
}
