# Middlewares y seguridad

## Registro
- Se aplican en `MiddlewareConfig.UseCustomMiddlewares` y se invocan desde `Program.cs` mediante `app.UseCustomMiddlewares()`.

## LogContextTraceMiddleware
- Enriquece el contexto de logging con informacion de traza (ver `Middlewares/LogContextTraceMiddleware.cs`).
- Util para correlacionar peticiones con Serilog y OpenTelemetry.

## PathSanitizationMiddleware
- Usa `Utils/PathSanitization.cs` para detectar rutas maliciosas (patrones regex contra saltos de directorio, variables no permitidas y rutas bloqueadas).
- Si detecta un patron sospechoso puede registrar o bloquear la peticion (revisa la logica del middleware).
- Para extender reglas agrega expresiones a `SPatterns` y actualiza `PathSanitizationTests` con nuevos escenarios.

## AddHeadersMiddleware
- Anade encabezados de seguridad como `Content-Security-Policy`, `Strict-Transport-Security`, `X-Content-Type-Options`.
- Refuerza politicas de cache y proteccion XSS en todas las respuestas.

## Buenas practicas
- Ajusta los encabezados segun el dominio corporativo (actualmente `*.tuya.com.co`).
- Versiona los cambios de sanitizacion junto con un conjunto de pruebas para evitar falsos positivos.
- Revalida los middlewares en ambientes de staging antes de moverlos a produccion.
