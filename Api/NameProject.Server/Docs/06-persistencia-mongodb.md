# Persistencia con MongoDB (EF Core Provider)

## Cliente Mongo
- `ConfigureServiceExtensions.AddMongoClient` registra un `MongoClient` singleton usando la cadena `MongoDb:StringConnection`.
- Se habilita `DiagnosticsActivityEventSubscriber` para trazas OpenTelemetry y correlación con logs.

## Contexto EF Core
- `Common.Infrastructure.ServiceExtensions.AddMongoService<TContext>` configura `DbContext` con `UseMongoDB`.
- `User.Infrastructure/UserDbContext.cs` define la colección `Users` y asigna `ToCollection("users")`.
- Agrega métodos estáticos para inicializar índices (`context.Users.Indexes.CreateOne(...)`) y llámalos durante el arranque del módulo.

## Repositorios
- `Common.Infrastructure.Repositories.GenericRepository` provee operaciones CRUD, paginación y soporte de includes.
- `User.Infrastructure.Repositories.UserRepository` hereda del repositorio genérico y expone métodos específicos (`GetAsync`, `InsertAsync`).
- `UnitOfWork` centraliza repositorios y guarda cambios via `DbContext.SaveChangesAsync`.

## Configuración de cadena de conexión
- Define la cadena en `appsettings.{Environment}.json` dentro de `ConnectionStrings.MongoDb:StringConnection`.
- Usa secretos seguros o variables de entorno en producción (`ConnectionStrings__MongoDb__StringConnection`).

## Uso desde handlers
- Los handlers de Application interactúan con `IUnitOfWork` (`CreateUserHandler`, `GetUsersHandler`).
- Los repositorios operan sobre entidades del módulo (`User.Domain.Entities.UserEntity`) y devuelven DTO a través de mapeadores.

## Estrategias adicionales
- Controla el tamaño de página por defecto vía `PaginationParams` para evitar consultas costosas.
- Documenta la consistencia eventual de MongoDB y considera usar filtros con índices para operaciones de lectura críticas.
- Automatiza datos semilla creando métodos `SeedAsync` por entorno cuando necesites información base.
