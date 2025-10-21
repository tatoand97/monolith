# Middlewares y seguridad

## Registro
- Se aplican en `MiddlewareConfig.UseCustomMiddlewares` y se invocan desde `Program.cs` mediante `app.UseCustomMiddlewares()`.

## LogContextTraceMiddleware
- Enriquece el contexto de logging con información de traza (ver `Middlewares/LogContextTraceMiddleware.cs`).
- Útil para correlacionar peticiones con Serilog y OpenTelemetry.

## PathSanitizationMiddleware
- Usa `Utils/PathSanitization.cs` para detectar rutas maliciosas (patrones regex contra saltos de directorio, variables no permitidas y rutas bloqueadas).
- Si detecta un patrón sospechoso puede registrar o bloquear la petición (revisa la lógica del middleware).
- Para extender reglas agrega expresiones a `SPatterns` y actualiza `PathSanitizationTests` con nuevos escenarios.

## AddHeadersMiddleware
- Añade encabezados de seguridad como `Content-Security-Policy`, `Strict-Transport-Security`, `X-Content-Type-Options`.
- Refuerza políticas de caché y protección XSS en todas las respuestas.

## Buenas prácticas
- Ajusta los encabezados según el dominio corporativo (actualmente `*.tuya.com.co`).
- Versiona los cambios de sanitización junto con un conjunto de pruebas para evitar falsos positivos.
- Revalida los middlewares en ambientes de staging antes de moverlos a producción.
