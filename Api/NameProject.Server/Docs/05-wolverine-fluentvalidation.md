# Comandos, consultas y validaciones (Wolverine + FluentValidation)

## Configuración en Program.cs
- `builder.Host.UseWolverine(...)` habilita Wolverine en modo mediator (`DurabilityMode.MediatorOnly`).
- `options.UseFluentValidation(RegistrationBehavior.ExplicitRegistration)` activa verificaciones de entrada automáticas.
- El pipeline registra `CustomFailureAction` (`Utils/Validation/CustomFailureAction.cs`) para transformar errores en `ModelValidationException`.
- Agrega nuevos ensamblados al discovery con `options.Discovery.IncludeAssembly({Feature}.Application.AssemblyReference.Assembly)`.

## Flujo de comandos
- Los comandos residen en `Modules/{Feature}/Application/Commands`.
- Los handlers reciben dependencias por constructor (ejemplo `CreateUserHandler` con `IUnitOfWork`).
- Como alternativa, puedes usar funciones estaticas decoradas con `[MessageHandler]`:
```csharp
[MessageHandler]
public static async Task<Response<RegisterResponse>> Handle(CreateUser command, IUnitOfWork unitOfWork, CancellationToken ct)
{
    var user = command.ToEntity();
    await unitOfWork.Users.InsertAsync(user, ct);
    await unitOfWork.SaveChangesAsync(ct);
    return Response<RegisterResponse>.Succeed(RegisterResponse.FromId(user.Id, user.Email), "Usuario registrado");
}
```
- Reutiliza `Common.Domain.Responses.Response<T>` para estandarizar mensajes éxito/error.

## Consultas con Wolverine
- Las consultas encapsulan parámetros (por ejemplo `GetUsers` hereda de una clase abstracta con `PaginationParams`).
- Los handlers devuelven DTO o `PagedList<T>` y se invocan desde endpoints con `IMessageBus.InvokeAsync<TResponse>(query)`.
- Puedes registrar middlewares Wolverine (policies) para agregar logging o métricas a cada mensaje.

## Validaciones con FluentValidation
- Registra validadores con `services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly)` dentro del módulo.
- Los validators se ejecutan antes de los handlers y los errores terminan en `ModelValidationException`, manejada por `GlobalExceptionHandler`.
- Para reglas complejas, usa `RuleFor(x => x.Email).EmailAddress().WithMessage("Mensaje claro")`.

## Recomendaciones
- Un validator por comando o consulta compleja mantiene reglas localizadas.
- Maneja excepciones esperadas dentro del handler devolviendo `Response.Fail` cuando no amerite lanzar.
- Crea pruebas para handlers y validators verificando mensajes y comportamiento con dependencias simuladas (`IUnitOfWork`, repositorios).
