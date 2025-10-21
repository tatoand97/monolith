# Arquitectura y estructura de carpetas

## Visión general
- `Api/NameProject.Server`: punto de entrada Web minimal API (`Program.cs`), configuración transversal y servicios compartidos.
- `Common`: librerías reutilizables (dominio, infraestructura y presentación).
- `Modules`: módulos verticales independientes (ejemplo `User`) con capas Application, Domain, Infrastructure, Presentation y Tests.

## Capas compartidas
- `Common/Common.Domain`: contratos genéricos (`Interfaces/IGenericRepository.cs`), modelos de respuesta y utilidades de paginación.
- `Common/Common.Infrastructure`: extensiones para MongoDB (`ServiceExtensions/ServiceExtensions.cs`) y repositorios genéricos.
- `Common/Common.Presentation`: contrato `IEndpoint` y utilidades para registrar endpoints dinámicamente (`Endpoint/EndpointExtensions.cs`).

## API y configuración
- Configuración transversal en `Api/NameProject.Server/Configs`: CORS, excepciones, telemetría, health checks, módulos y middlewares.
- `ServiceCollections/`: extensiones de DI para Swagger, MongoDB, mapeo de endpoints y clientes compartidos.
- `Middlewares/`: middleware personalizados declarados en `MiddlewareConfig.UseCustomMiddlewares`.
- `Utils/`: utilidades para logging, validación y health checks.

## Módulos verticales
Cada módulo sigue el patrón:
- `Domain`: entidades, interfaces de repositorio, excepciones.
- `Application`: comandos y consultas, DTO, mapeadores, validadores y handlers Wolverine.
- `Infrastructure`: contexto de datos y repositorios concretos.
- `Presentation`: endpoints minimalistas y módulo de DI (`{Feature}Module.cs`) que registra Application e Infrastructure.
- `Tests`: pruebas unitarias y de integración ligadas al dominio del módulo.

## Referencias entre proyectos
- `Api` referencia `Common.*` y las capas `Modules/{Feature}/Presentation`.
- `Presentation` referencia `Application`, `Application` referencia `Domain` y `Infrastructure`; `Infrastructure` referencia `Domain` y `Common.Infrastructure`.
- Usa `AssemblyReference.cs` en cada capa para resolver ensamblados en tiempo de ejecución sin dependencias circulares.
