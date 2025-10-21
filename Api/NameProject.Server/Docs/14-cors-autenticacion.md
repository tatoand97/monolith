# CORS, autenticación y seguridad de peticiones

## CORS
- La política se define en `Configs/CorsConfig.cs` usando la constante `MyPolicy`.
- Actualmente permite cualquier origen (`AllowAnyOrigin`). Cámbialo por `WithOrigins("https://tu-dominio")` en ambientes productivos.
- Usa `AllowAnyHeader` y `AllowAnyMethod` solo cuando sea necesario; preferir listas explícitas si conoces los métodos esperados.
- Se registra en `Program.cs` con `builder.Services.SetupCors()` y se aplica con `app.UseCors(CorsConfig.MyPolicy)`.

## Autenticación (pendiente)
- El template aún no habilita autenticación. Para agregarla:
  1. Incluye `builder.Services.AddAuthentication().AddJwtBearer(...)` en `Program.cs`.
  2. Protege endpoints agregando `.RequireAuthorization()` dentro de `MapEndpoint`.
  3. Configura proveedores de identidad y secret keys en `appsettings`.
- Documenta los scopes y claims requeridos por módulo.

## Seguridad adicional de solicitudes
- Usa `PathSanitizationMiddleware` para bloquear rutas maliciosas antes de que lleguen a la lógica de negocio.
- `AddHeadersMiddleware` aplica encabezados de endurecimiento (CSP, HSTS, X-Content-Type-Options). Ajusta dominios permitidos en CSP para tu organización.
- Considera agregar `RateLimiting` (`AddRateLimiter`) si esperas tráfico elevado o necesitas mitigar ataques de fuerza bruta.

## Recomendaciones
- Mantén las opciones de CORS alineadas con los dominios declarados en la plataforma front-end.
- Revisa las políticas tras cada despliegue y agrega pruebas E2E que validen preflight (`OPTIONS`).
- Si habilitas autenticación, registra un health check adicional que verifique la comunicación con el proveedor de identidad.
