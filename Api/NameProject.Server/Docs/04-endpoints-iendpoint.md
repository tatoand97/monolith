# Endpoints minimalistas con `IEndpoint`

## Contrato común
- Los endpoints implementan `Common.Presentation.Endpoint.IEndpoint`.
- El método `MapEndpoint(IEndpointRouteBuilder)` registra rutas y metadatos.

## Registro dinámico
- `WebApplication.MapEndpoints` (ver `Common/Common.Presentation/Endpoint/EndpointExtensions.cs`) escanea tipos `IEndpoint` en cada ensamblado.
- `MapModuleEndpoints` agrega los endpoints de cada módulo durante el arranque (`Api/NameProject.Server/ServiceCollections/EndpointMappingExtensions.cs`).
- Puedes agrupar rutas personalizadas llamando `app.MapEndpoints(assembly, app.MapGroup("/prefijo"))`.

## Ejemplo: listado de usuarios
Archivo `Modules/User/Presentation/Users/GetUsers.cs`:
```csharp
public class GetUsers(IMessageBus messageBus) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder e)
    {
        e.MapGet("/users", Handle)
            .AllowAnonymous()
            .WithName("Users.Get");
    }

    private async Task<IResult> Handle([AsParameters] GetUsers query, CancellationToken ct)
    {
        var pagedUsers = await messageBus.InvokeAsync<PagedList<UserDto>>(query, ct);
        var response = PagedResponse<UserDto>.FromPagedList(pagedUsers, "Usuarios obtenidos exitosamente");
        return Results.Ok(response);
    }
}
```

## Buenas prácticas
- Inyecta dependencias en el constructor del endpoint (record) para aprovechar DI.
- Usa `[AsParameters]` para mapear objetos de consulta o comando.
- Reutiliza tipos de respuesta comunes (`PagedResponse`, `Response`) para estandarizar contratos.
- Define nombres de rutas con `WithName` para integración con Swagger y link generation.
