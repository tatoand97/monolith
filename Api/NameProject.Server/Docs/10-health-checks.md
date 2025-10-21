# Health checks

## Configuracion de servicios
- `Configs/HealthcheckConfig.ConfigureHealthCheckServices` registra health checks en DI.
- Actualmente anade `CosmosMongoHealthCheck` con etiqueta `ready`.

## Endpoints expuestos
- `ConfigureHealthChecks` mapea:
  - `/health/live`: verifica unicamente que la app responda (sin ejecutar checks).
  - `/health/ready`: ejecuta los checks con etiqueta `ready`.
- Se registran en `Program.cs` tras `app.UseRouting()` usando `builder.Services.ConfigureHealthCheckServices()`.

## CosmosMongoHealthCheck
- Ubicado en `Utils/HealthChecks/CosmosMongoHealthCheck.cs`.
- Intenta conectarse a MongoDB utilizando el `MongoClient` configurado.
- Devuelve `Healthy` si la conexion es exitosa, `Unhealthy` con detalles en caso contrario.

## Integracion con monitoreo
- Configura Kubernetes, App Service o balanceadores para consultar `/health/ready` antes de enrutar trafico.
- `/health/live` es util para sondas de liveness (reinicio rapido si la app se bloquea).

## Como agregar nuevos checks
1. Crea una clase que implemente `IHealthCheck`.
2. Registra el check en `ConfigureHealthCheckServices` con `tags` segun el escenario:
   ```csharp
   services.AddHealthChecks()
       .AddCheck<MyDependencyHealthCheck>("my-dependency", tags: new[] { "ready" });
   ```
3. Actualiza la documentacion o dashboards para incluir el nuevo endpoint.

## Buenas practicas
- Etiqueta con `tags: ["live"]`, `["ready"]` o personalizadas para controlar que expone cada ruta.
- Devuelve mensajes explicitos en `HealthCheckResult` para facilitar el diagnostico.
- Automatiza pruebas de smoke que consulten `/health/ready` despues de cada despliegue.
