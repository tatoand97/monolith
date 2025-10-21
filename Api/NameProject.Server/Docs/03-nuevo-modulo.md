# Creación y registro de un nuevo módulo

## 1. Estructura inicial
1. Crea la carpeta `Modules/{Nombre}` con subcarpetas `Domain`, `Application`, `Infrastructure`, `Presentation`, `Tests`.
2. Agrega proyectos `.csproj` replicando las referencias del módulo User como plantilla (ver `Modules/User/*/*.csproj`).
3. Ajusta `AssemblyReference.cs` en cada capa para devolver `typeof(AssemblyReference).Assembly`.

## 2. Definir dominio e infraestructura
- Declara entidades y contratos en `Domain` (interfaces tipo `I{Feature}Repository`, `IUnitOfWork`).
- Implementa el contexto y repositorios en `Infrastructure` reutilizando `Common.Infrastructure.Repositories.GenericRepository`.
- Registra el contexto y unit of work dentro del módulo (ver `UserModule.AddInfrastructure`).

## 3. Aplicación, Wolverine y validaciones
- Crea comandos, consultas y DTO en `Application`.
- Implementa handlers Wolverine (clases simples o decoradas con atributos como `[MessageHandler]`).
- Registra validadores con FluentValidation dentro de `Setup{Feature}Module` (`services.AddValidatorsFromAssembly(...)`).
- Si el módulo necesita comportamientos Wolverine adicionales, agrega el ensamblado con `options.Discovery.IncludeAssembly({Feature}.Application.AssemblyReference.Assembly)` en `Program.cs`.

## 4. Endpoints de presentación
- Implementa endpoints minimalistas que implementen `Common.Presentation.Endpoint.IEndpoint`.
- Usa `IMessageBus` para publicar comandos o consultas al pipeline Wolverine.
- Agrega un módulo DI tipo `Setup{Feature}Module` que registre Application e Infrastructure y exponga configuraciones necesarias.

## 5. Registro en la API
- Agrega el módulo al método `AddModules` en `Api/NameProject.Server/Configs/ModulesConfig.cs`.
- Incluye el ensamblado en `Api/NameProject.Server/ServiceCollections/EndpointMappingExtensions.cs` para mapear endpoints automáticamente.
- Si tu módulo requiere configuración de appsettings, valida que `ModulesAppSettingsConfig.AddModulesConfiguration` la cargue.

## 6. Configuración y appsettings
- Si el módulo requiere configuración, agrega secciones en `appsettings.{Environment}.json` y documenta valores esperados.
- Define variables de entorno para secretos (por ejemplo `ConnectionStrings__MyModule__Primary`).

## 7. Pruebas
- Agrega un proyecto de pruebas en `Modules/{Nombre}/Tests`, configurando referencias a las capas necesarias.
- Crea casos de prueba siguiendo el ejemplo `Modules/User/Tests/User.Test` e incluye pruebas para handlers, validadores y mapeos.
