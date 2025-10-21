# Observabilidad y logging

## Serilog
- Configurado en `Configs/SerilogConfig.cs` y habilitado con `builder.Host.UseSerilogCustom()`.
- Enriquecedores:
  - `ActivityEnricher` agrega `trace_id` y `span_id`.
  - `Serilog.Enrichers.Sensitive` mascara datos sensibles (email, tarjeta, IBAN) con `MaskingMode.Globally`.
- Salidas:
  - Consola.
  - Exportador OpenTelemetry (`WriteTo.OpenTelemetry`) filtrado al nivel `Warning`.
- Variables de entorno relevantes:
  - `OTEL_SERVICE_NAME` agregado como propiedad `service.name`.
  - `OTEL_EXPORTER_OTLP_ENDPOINT` con la URL del colector OTLP.
- Configuracion de appsettings de ejemplo:
```json
{
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" }
    ]
  },
  "OpenTelemetry": {
    "Endpoint": "https://otel-collector"
  }
}
```

## OpenTelemetry
- Configurado en `Configs/OpenTelemetryConfig.cs` mediante `services.ConfigureOpenTelemetry()`.
- Metricas: runtime, proceso, ASP.NET Core (`AddRuntimeInstrumentation`, `AddAspNetCoreInstrumentation`, etc.).
- Trazas: HTTP, EF Core, gRPC y MongoDB (`MongoDB.Driver.Core.Extensions.DiagnosticSources`).
- Exportadores OTLP configurables via `OTEL_EXPORTER_OTLP_ENDPOINT`.

## Health y diagnosticos
- Los health checks (`Configs/HealthcheckConfig.cs`) sirven como senales de disponibilidad para plataformas de monitoreo.
- `CosmosMongoHealthCheck` verifica la conectividad a MongoDB y puede instrumentarse con etiquetas personalizadas.

## Recomendaciones
- Ajusta los niveles de logging por entorno en `appsettings.{Environment}.json`.
- Emite metricas personalizadas registrando `Meter` adicionales si es necesario.
- Configura dashboards en tu colector OTLP (Grafana, Azure Monitor, etc.) usando los atributos exportados.
