# Manejo de errores y Problem Details

## Registro global
- `Configs/ExceptionsConfig.SetupExceptions` habilita `AddProblemDetails` y el handler global `GlobalExceptionHandler`.
- `CustomFailureAction` convierte errores de FluentValidation en `ModelValidationException`.

## Pipeline de excepciones
- Cuando se lanza `ModelValidationException`, el handler responde con `400` y un objeto JSON con `message` y `errors`.
- `UnauthorizedAccessException` se transforma en `401` con mensaje de texto plano.
- Excepciones no controladas devuelven `500` con mensaje generico.

## Extender el handler
1. Agrega nuevos `case` en `GlobalExceptionHandler` para excepciones personalizadas.
2. Usa `httpContext.Response.WriteAsJsonAsync` para mantener compatibilidad con Problem Details.
3. Registra errores criticos con `logger.LogError` incluyendo contexto adicional.

## Integracion con Problem Details
- `AddProblemDetails` agrega metadatos estandar (tipo, titulo, status) cuando se utiliza `ProblemDetails`.
- Para exponer mas informacion, crea un DTO propio y devuelvelo desde el handler (`WriteAsJsonAsync`).
- Mantiene los contractos ligeros en produccion, evitando enviar stack traces.

## Buenas practicas
- Evita capturar excepciones silenciosamente en handlers Wolverine; deja que `GlobalExceptionHandler` maneje errores inesperados.
- Documenta los codigos de estado esperados por endpoint y agrega pruebas que verifiquen las respuestas de error.
- Usa telemetria y logs (Serilog + OpenTelemetry) para correlacionar excepciones con trazas.
