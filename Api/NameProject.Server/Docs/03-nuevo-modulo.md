# Creacion y registro de un nuevo modulo

## 1. Estructura inicial
1. Crea la carpeta `Modules/{Nombre}` con subcarpetas `Domain`, `Application`, `Infrastructure`, `Presentation`, `Tests`.
2. Agrega proyectos `.csproj` replicando las referencias del modulo User como plantilla (ver `Modules/User/*/*.csproj`).
3. Ajusta `AssemblyReference.cs` en cada capa para devolver `typeof(AssemblyReference).Assembly`.

## 2. Definir dominio e infraestructura
- Declara entidades y contratos en `Domain` (interfaces tipo `I{Feature}Repository`, `IUnitOfWork`).
- Implementa el contexto y repositorios en `Infrastructure` reutilizando `Common.Infrastructure.Repositories.GenericRepository`.
- Registra el contexto y unit of work dentro del modulo (ver `UserModule.AddInfrastructure`).

## 3. Aplicacion, Wolverine y validaciones
- Crea comandos, consultas y DTO en `Application`.
- Implementa handlers Wolverine (clases simples o decoradas con atributos como `[MessageHandler]`).
- Registra validadores con FluentValidation dentro de `Setup{Feature}Module` (`services.AddValidatorsFromAssembly(...)`).
- Si el modulo necesita comportamientos Wolverine adicionales, agrega la asamblea con `options.Discovery.IncludeAssembly({Feature}.Application.AssemblyReference.Assembly)` en `Program.cs`.

## 4. Endpoints de presentacion
- Implementa endpoints minimalistas que implementen `Common.Presentation.Endpoint.IEndpoint`.
- Usa `IMessageBus` para publicar comandos o consultas al pipeline Wolverine.
- Agrega un modulo DI tipo `Setup{Feature}Module` que registre Application e Infrastructure y exponga configuraciones necesarias.

## 5. Registro en la API
- Agrega el modulo al metodo `AddModules` en `Api/NameProject.Server/Configs/ModulesConfig.cs`.
- Incluye la asamblea en `Api/NameProject.Server/ServiceCollections/EndpointMappingExtensions.cs` para mapear endpoints automaticamente.
- Si tu modulo requiere configuracion de appsettings, valida que `ModulesAppSettingsConfig.AddModulesConfiguration` la cargue.

## 6. Configuracion y appsettings
- Si el modulo requiere configuracion, agrega secciones en `appsettings.{Environment}.json` y documenta valores esperados.
- Define variables de entorno para secretos (por ejemplo `ConnectionStrings__MyModule__Primary`).

## 7. Pruebas
- Agrega un proyecto de pruebas en `Modules/{Nombre}/Tests`, configurando referencias a las capas necesarias.
- Crea casos de prueba siguiendo el ejemplo `Modules/User/Tests/User.Test` e incluye pruebas para handlers, validadores y mapeos.
