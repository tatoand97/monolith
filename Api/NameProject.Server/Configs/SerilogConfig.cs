using Serilog;
using Serilog.Enrichers.Sensitive;

namespace NameProject.Server.Configs;

/// <summary>
/// Provides extension methods for configuring Serilog in the application.
/// </summary>
public static class SerilogConfig
{
    /// <summary>
    /// Configures Serilog logging for the application and adds it to the provided host builder.
    /// </summary>
    /// <param name="hostBuilder">The IHostBuilder instance to which the Serilog logger and logging configuration are added.</param>
    /// <returns>The modified IHostBuilder with Serilog logging configured.</returns>
    public static IHostBuilder UseSerilogCustom(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog();

        hostBuilder.ConfigureServices(services =>
        {
            services.AddLoggingSerilog();
        });
        
        return hostBuilder;
    }


    /// <summary>
    /// Configures Serilog logging for the application and registers it in the service collection.
    /// </summary>
    /// <param name="services">The service collection to which the Serilog logger is added.</param>
    /// <returns>The modified service collection with Serilog logging configured.</returns>
    private static IServiceCollection AddLoggingSerilog(this IServiceCollection services)
    {
        const string
            serviceName = "NameProject"; //Aquí se configura el nombre del servicio como aparece en el enviroment
        var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
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
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = serviceName,
                };
            })
            .CreateLogger();
        
        return services;
    }
}
