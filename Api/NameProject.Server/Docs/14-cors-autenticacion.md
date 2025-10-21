# CORS, autenticacion y seguridad de peticiones

## CORS
- La politica se define en `Configs/CorsConfig.cs` usando la constante `MyPolicy`.
- Actualmente permite cualquier origen (`AllowAnyOrigin`). Cambialo por `WithOrigins("https://tu-dominio")` en ambientes productivos.
- Use `AllowAnyHeader` y `AllowAnyMethod` solo cuando sea necesario; preferir listas explicitas si conoces los metodos esperados.
- Se registra en `Program.cs` con `builder.Services.SetupCors()` y se aplica con `app.UseCors(CorsConfig.MyPolicy)`.

## Autenticacion (pendiente)
- El template aun no habilita autenticacion. Para agregarla:
  1. Incluye `builder.Services.AddAuthentication().AddJwtBearer(...)` en `Program.cs`.
  2. Protege endpoints agregando `.RequireAuthorization()` dentro de `MapEndpoint`.
  3. Configura proveedores de identidad y secret keys en `appsettings`.
- Documenta los scopes y claims requeridos por modulo.

## Seguridad adicional de solicitudes
- Usa `PathSanitizationMiddleware` para bloquear rutas maliciosas antes de que lleguen a la logica de negocio.
- `AddHeadersMiddleware` aplica encabezados de endurecimiento (CSP, HSTS, X-Content-Type-Options). Ajusta dominios permitidos en CSP para tu organizacion.
- Considera agregar `RateLimiting` (`AddRateLimiter`) si esperas trafico elevado o necesitas mitigar ataques de fuerza bruta.

## Recomendaciones
- Mantiene las opciones de CORS alineadas con los dominios declarados en la plataforma front-end.
- Revisa las politicas tras cada despliegue y agrega pruebas E2E que validen preflight (`OPTIONS`).
- Si habilitas autenticacion, registra un health check adicional que verifique la comunicacion con el proveedor de identidad.
