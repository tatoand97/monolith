# Despliegue y CI/CD

## Publicacion manual
1. Ejecuta pruebas y analisis estatico:
   ```bash
   dotnet test Monolith.sln
   dotnet build Monolith.sln --configuration Release
   ```
2. Publica el proyecto API:
   ```bash
   dotnet publish Api/NameProject.Server/NameProject.Server.csproj --configuration Release --output publish
   ```
3. Empaqueta la carpeta `publish` para desplegarla en App Service, contenedor o VM.

## Pipeline recomendado
- Paso 1: restaurar (`dotnet restore`).
- Paso 2: compilar y ejecutar pruebas (`dotnet build`, `dotnet test --collect:"XPlat Code Coverage"`).
- Paso 3: publicar artefacto (`dotnet publish`).
- Paso 4: ejecutar pruebas de humo contra `/health/ready` usando la version publicada.
- Paso 5: desplegar a ambiente objetivo (Azure Web App, Kubernetes, etc.).

## Variables obligatorias
- Configura `ASPNETCORE_ENVIRONMENT`, `ConnectionStrings__MongoDb__StringConnection`, `DatabaseName`, `OTEL_*`.
- Usa stores seguros (Key Vault, GitHub Actions secrets, Azure Pipeline variables) para credenciales.

## Tareas posteriores al despliegue
- Verifica health checks (GET `/health/live`, `/health/ready`).
- Revisa logs de Serilog/OpenTelemetry para confirmar conectividad.
- Ejecuta pruebas de smoke (por ejemplo un `GET /users` en entorno de pruebas).

## Buenas practicas
- Incluye politicas de aprovacion manual antes de publicar en produccion.
- Versiona artefactos con numero de build o commit sha para trazabilidad.
- Automatiza rollback monitoreando health checks y metrica de errores.
