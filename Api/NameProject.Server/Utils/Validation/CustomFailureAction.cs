using Common.Domain.Exceptions;
using FluentValidation.Results;
using Wolverine.FluentValidation;

namespace NameProject.Server.Utils.Validation;

public class CustomFailureAction<T> : IFailureAction<T>
{
    public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
    {
        var errorList = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}");
        throw new ModelValidationException($"Validation failed: {string.Join(" | ", errorList)}");
    }
}