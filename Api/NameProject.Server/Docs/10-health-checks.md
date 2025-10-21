# Health checks

## Configuración de servicios
- `Configs/HealthcheckConfig.ConfigureHealthCheckServices` registra health checks en DI.
- Actualmente añade `CosmosMongoHealthCheck` con etiqueta `ready`.

## Endpoints expuestos
- `ConfigureHealthChecks` mapea:
  - `/health/live`: verifica únicamente que la app responda (sin ejecutar checks).
  - `/health/ready`: ejecuta los checks con etiqueta `ready`.
- Se registran en `Program.cs` tras `app.UseRouting()` usando `builder.Services.ConfigureHealthCheckServices()`.

## CosmosMongoHealthCheck
- Ubicado en `Utils/HealthChecks/CosmosMongoHealthCheck.cs`.
- Intenta conectarse a MongoDB utilizando el `MongoClient` configurado.
- Devuelve `Healthy` si la conexión es exitosa, `Unhealthy` con detalles en caso contrario.

## Integración con monitoreo
- Configura Kubernetes, App Service o balanceadores para consultar `/health/ready` antes de enrutar tráfico.
- `/health/live` es útil para sondas de liveness (reinicio rápido si la app se bloquea).

## Cómo agregar nuevos checks
1. Crea una clase que implemente `IHealthCheck`.
2. Registra el check en `ConfigureHealthCheckServices` con `tags` segun el escenario:
   ```csharp
   services.AddHealthChecks()
       .AddCheck<MyDependencyHealthCheck>("my-dependency", tags: new[] { "ready" });
   ```
3. Actualiza la documentación o dashboards para incluir el nuevo endpoint.

## Buenas prácticas
- Etiqueta con `tags: ["live"]`, `["ready"]` o personalizadas para controlar que expone cada ruta.
- Devuelve mensajes explícitos en `HealthCheckResult` para facilitar el diagnóstico.
- Automatiza pruebas de smoke que consulten `/health/ready` después de cada despliegue.
