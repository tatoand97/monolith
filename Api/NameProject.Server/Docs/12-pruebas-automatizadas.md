# Pruebas automatizadas

## Estructura actual
- Las pruebas residen en `Modules/{Feature}/Tests`.
- El ejemplo disponible (`Modules/User/Tests/User.Test`) usa xUnit (`User.Test.csproj`).
- Cada proyecto de pruebas referencia las capas del módulo para reutilizar DTO, handlers y utilidades.

## Caso de ejemplo
- `PathSanitizationTests.cs` valida los patrones de `Api/NameProject.Server/Utils/PathSanitization.cs`.
- Usa teorías (`[Theory]`) para cubrir múltiples rutas maliciosas y seguras.

## Crear nuevas pruebas
1. Agrega un proyecto de pruebas por módulo (`dotnet new xunit -o Modules/{Feature}/Tests/{Feature}.Test`).
2. Referencia las capas necesarias (Application, Domain, Infrastructure, API utilidades) desde el `.csproj`.
3. Organiza los tests por tema (Handlers, Validators, Middlewares, Integración).
4. Usa `WebApplicationFactory` o `TestServer` para validar middleware y pipeline HTTP cuando sea necesario.

## Cobertura y CI
- Ejecuta pruebas locales con `dotnet test Monolith.sln`.
- Genera cobertura:
  ```bash
  dotnet test Monolith.sln --collect:"XPlat Code Coverage"
  ```
- Publica los reportes (`coverage.cobertura.xml`) en tu pipeline CI para seguimiento continuo.

## Recomendaciones
- Cubre validadores de FluentValidation para asegurar mensajes de error claros.
- Mockea dependencias de infraestructura cuando pruebes handlers (`IUnitOfWork`, repositorios) usando frameworks como Moq o NSubstitute.
- Para endpoints, crea pruebas de integración que llamen a `IMessageBus` y validen respuestas `Response<T>`.
- Integra `dotnet test` en CI/CD y falla el pipeline si no se cumplen los criterios de cobertura definidos.
