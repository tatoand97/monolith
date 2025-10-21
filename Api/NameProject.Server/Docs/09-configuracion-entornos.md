# Configuracion y entornos

## Archivos appsettings
- `ModulesAppSettingsConfig.AddModulesConfiguration` carga `appsettings.json` y `appsettings.{Environment}.json`.
- Define `ASPNETCORE_ENVIRONMENT` antes de arrancar (`Development`, `Certification`, `Production`).
- Variables importantes:
  - `DatabaseName` para registrar el contexto Mongo en `UserModule`.
  - `ConnectionStrings.MongoDb:StringConnection` para `MongoClient`.
  - Secciones de observabilidad (`OTEL_*`) y Azure App Configuration si aplica.

## Azure App Configuration (opcional)
- Habilita `builder.ConfigureAppConfiguration()` en `Program.cs` si quieres usar `ConfigureServiceExtensions.ConfigureAppConfiguration`.
- Requiere:
  - `AppConfiguration:Endpoint` con la URI o cadena de conexion.
  - Permisos para `DefaultAzureCredential` (Managed Identity o Azure CLI).
  - Clave sentinel `Settings.Sentinel` para refresco de configuracion.

## Variables de entorno clave
```
ASPNETCORE_ENVIRONMENT=Development
OTEL_SERVICE_NAME=NameProject.Api
OTEL_EXPORTER_OTLP_ENDPOINT=https://otel-collector
ConnectionStrings__MongoDb__StringConnection=mongodb://user:pass@host/db
DatabaseName=NameProject
```

## Matriz sugerida por entorno
- Development: valores locales, `ASPNETCORE_ENVIRONMENT=Development`, strings a localhost, logs verbose.
- Certification: configuraciones similares a produccion pero con credenciales de pruebas, habilita health checks y telemetria.
- Production: usa secretos gestionados (Key Vault, App Secrets), define politicas de rotacion para credenciales y endpoints reales.

## Recomendaciones
- Nunca almacenes credenciales reales en el repositorio; usa secretos locales o Key Vault.
- Documenta valores por entorno en un repositorio seguro o en la wiki interna.
- Valida configuraciones en CI/CD ejecutando `dotnet run` o pruebas de humo con las variables correspondientes.
