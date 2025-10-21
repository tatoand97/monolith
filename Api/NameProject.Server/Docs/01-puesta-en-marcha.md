# Guia de puesta en marcha

## Requisitos previos
- .NET SDK 8.0 o superior (`dotnet --version`).
- Variables de entorno opcionales para observabilidad (`OTEL_SERVICE_NAME`, `OTEL_EXPORTER_OTLP_ENDPOINT`).

## Restaurar dependencias
```bash
dotnet restore Monolith.sln
```

## Configurar la aplicacion
- Ajusta las cadenas de conexion y parametros en `Api/NameProject.Server/appsettings.Development.json`.
- Si usas otro entorno, crea o edita el archivo `appsettings.{Environment}.json`.
- Define `ASPNETCORE_ENVIRONMENT` antes de ejecutar (`Development`, `Certification`, `Production`).
- Mantiene secretos y credenciales fuera de los archivos JSON. Usa variables de entorno o proveedores seguros (Azure Key Vault, dotnet user-secrets).

## Ejecutar el host web
```bash
dotnet run --project Api/NameProject.Server/NameProject.Server.csproj
```
El servicio expone Swagger en `/swagger` cuando `ASPNETCORE_ENVIRONMENT=Development`.

## Publicar para produccion
```bash
dotnet publish Api/NameProject.Server/NameProject.Server.csproj --configuration Release --output publish
```
- Revisa el contenido de la carpeta `publish` y despliega el paquete a tu plataforma (App Service, contenedor, VM).
- Antes de publicar, valida que `OTEL_*`, `ASPNETCORE_ENVIRONMENT` y cadenas de conexion esten configuradas en el entorno destino.

## Verificaciones rapidas
- Endpoint de salud: `GET /health/live` y `GET /health/ready`.
- Logs en consola enriquecidos por Serilog (`Configs/SerilogConfig.cs`).
- Endpoints del modulo User (`Modules/User/Presentation/Users/`).
