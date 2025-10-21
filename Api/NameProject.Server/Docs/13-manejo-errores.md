# Manejo de errores y Problem Details

## Registro global
- `Configs/ExceptionsConfig.SetupExceptions` habilita `AddProblemDetails` y el handler global `GlobalExceptionHandler`.
- `CustomFailureAction` convierte errores de FluentValidation en `ModelValidationException`.

## Pipeline de excepciones
- Cuando se lanza `ModelValidationException`, el handler responde con `400` y un objeto JSON con `message` y `errors`.
- `UnauthorizedAccessException` se transforma en `401` con mensaje de texto plano.
- Excepciones no controladas devuelven `500` con mensaje genérico.

## Extender el handler
1. Agrega nuevos `case` en `GlobalExceptionHandler` para excepciones personalizadas.
2. Usa `httpContext.Response.WriteAsJsonAsync` para mantener compatibilidad con Problem Details.
3. Registra errores criticos con `logger.LogError` incluyendo contexto adicional.

## Integración con Problem Details
- `AddProblemDetails` agrega metadatos estándar (tipo, título, status) cuando se utiliza `ProblemDetails`.
- Para exponer más información, crea un DTO propio y devuélvelo desde el handler (`WriteAsJsonAsync`).
- Mantén los contratos ligeros en producción, evitando enviar stack traces.

## Buenas prácticas
- Evita capturar excepciones silenciosamente en handlers Wolverine; deja que `GlobalExceptionHandler` maneje errores inesperados.
- Documenta los códigos de estado esperados por endpoint y agrega pruebas que verifiquen las respuestas de error.
- Usa telemetría y logs (Serilog + OpenTelemetry) para correlacionar excepciones con trazas.
