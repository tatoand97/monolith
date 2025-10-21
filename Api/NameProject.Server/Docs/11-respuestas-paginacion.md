# Respuestas comunes y paginacion

## Modelos de respuesta
- `Common.Domain.Responses.Response<T>`: estructura estandar con `Success`, `Message`, `Data`, `Errors`.
- `Common.Domain.Responses.PagedResponse<T>`: hereda de `Response` incorporando `PageNumber`, `PageSize`, `TotalCount`, `Items`.

## Utilidades de paginacion
- `Common.Domain.Pagination.PaginationParams`: pagina y tamano con valores por defecto.
- `Common.Domain.Pagination.PagedList<T>`: representa resultados paginados desde repositorios.

## Del repositorio a la API
1. Los repositorios devuelven `PagedList<TEntity>` (`GenericRepository.ListPagedAsync`).
2. Los handlers convierten entidades a DTO y crean una nueva `PagedList<DTO>`.
3. Los endpoints exponen un `PagedResponse<T>` (ejemplo `Presentation/Users/GetUsers.cs`).

## Ejemplo de uso
Handler `GetUsersHandler`:
```csharp
var pagedEntities = await unitOfWork.Users.ListPagedAsync(paginationParams, ct: ct);
var dtoItems = pagedEntities.Items.ToDtoList();
return new PagedList<UserDto>(dtoItems, pagedEntities.TotalCount, pagedEntities.PageNumber, pagedEntities.PageSize);
```
Endpoint `GetUsers`:
```csharp
var response = PagedResponse<UserDto>.FromPagedList(pagedUsers, "Usuarios obtenidos exitosamente");
return Results.Ok(response);
```

## Contrato JSON esperado
```json
{
  "success": true,
  "message": "Usuarios obtenidos exitosamente",
  "data": [
    { "id": "guid", "email": "user@example.com", "name": "Name" }
  ],
  "errors": [],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 42
}
```

## Recomendaciones
- Propaga mensajes claros en `Response<T>.Succeed` y `Response<T>.Fail`.
- Mantiene un tamano de pagina por defecto razonable y permitido por la base de datos.
- Evita exponer entidades de dominio directamente; usa DTO dedicados.
