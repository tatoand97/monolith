# Persistencia con MongoDB (EF Core Provider)

## Cliente Mongo
- `ConfigureServiceExtensions.AddMongoClient` registra un `MongoClient` singleton usando la cadena `MongoDb:StringConnection`.
- Se habilita `DiagnosticsActivityEventSubscriber` para trazas OpenTelemetry y correlacion con logs.

## Contexto EF Core
- `Common.Infrastructure.ServiceExtensions.AddMongoService<TContext>` configura `DbContext` con `UseMongoDB`.
- `User.Infrastructure/UserDbContext.cs` define la coleccion `Users` y asigna `ToCollection("users")`.
- Agrega metodos estaticos para inicializar indices (`context.Users.Indexes.CreateOne(...)`) y llamalos durante el arranque del modulo.

## Repositorios
- `Common.Infrastructure.Repositories.GenericRepository` provee operaciones CRUD, paginacion y soporte de includes.
- `User.Infrastructure.Repositories.UserRepository` hereda del repositorio generico y expone metodos especificos (`GetAsync`, `InsertAsync`).
- `UnitOfWork` centraliza repositorios y guarda cambios via `DbContext.SaveChangesAsync`.

## Configuracion de cadena de conexion
- Define la cadena en `appsettings.{Environment}.json` dentro de `ConnectionStrings.MongoDb:StringConnection`.
- Usa secretos seguros o variables de entorno en produccion (`ConnectionStrings__MongoDb__StringConnection`).

## Uso desde handlers
- Los handlers de Application interactuan con `IUnitOfWork` (`CreateUserHandler`, `GetUsersHandler`).
- Los repositorios operan sobre entidades del modulo (`User.Domain.Entities.UserEntity`) y devuelven DTO a traves de mapeadores.

## Estrategias adicionales
- Controla el tamano de pagina por defecto via `PaginationParams` para evitar consultas costosas.
- Documenta la consistencia eventual de MongoDB y considera usar filtros con indices para operaciones de lectura criticas.
- Automatiza datos semilla creando metodos `SeedAsync` por entorno cuando necesites informacion base.
