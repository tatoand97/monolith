# Arquitectura y estructura de carpetas

## Vision general
- `Api/NameProject.Server`: punto de entrada Web minimal API (`Program.cs`), configuracion transversal y servicios compartidos.
- `Common`: librerias reutilizables (dominio, infraestructura y presentacion).
- `Modules`: modulos verticales independientes (ejemplo `User`) con capas Application, Domain, Infrastructure, Presentation y Tests.

## Capas compartidas
- `Common/Common.Domain`: contratos genericos (`Interfaces/IGenericRepository.cs`), modelos de respuesta y utilidades de paginacion.
- `Common/Common.Infrastructure`: extensiones para MongoDB (`ServiceExtensions/ServiceExtensions.cs`) y repositorios genericos.
- `Common/Common.Presentation`: contrato `IEndpoint` y utilidades para registrar endpoints dinamicamente (`Endpoint/EndpointExtensions.cs`).

## API y configuracion
- Configuracion transversal en `Api/NameProject.Server/Configs`: CORS, excepciones, telemetria, health checks, modulos y middlewares.
- `ServiceCollections/`: extensiones de DI para Swagger, MongoDB, mapeo de endpoints y clientes compartidos.
- `Middlewares/`: middleware personalizados declarados en `MiddlewareConfig.UseCustomMiddlewares`.
- `Utils/`: utilidades para logging, validacion y health checks.

## Modulos verticales
Cada modulo sigue el patron:
- `Domain`: entidades, interfaces de repositorio, excepciones.
- `Application`: comandos y consultas, DTO, mapeadores, validadores y handlers Wolverine.
- `Infrastructure`: contexto de datos y repositorios concretos.
- `Presentation`: endpoints minimalistas y modulo de DI (`{Feature}Module.cs`) que registra Application e Infrastructure.
- `Tests`: pruebas unitarias y de integracion ligadas al dominio del modulo.

## Referencias entre proyectos
- `Api` referencia `Common.*` y las capas `Modules/{Feature}/Presentation`.
- `Presentation` referencia `Application`, `Application` referencia `Domain` y `Infrastructure`; `Infrastructure` referencia `Domain` y `Common.Infrastructure`.
- Usa `AssemblyReference.cs` en cada capa para resolver ensamblados en tiempo de ejecucion sin dependencias circulares.
