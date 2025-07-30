namespace Common.Domain.Exceptions;

public class ModelValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ModelValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }

    public ModelValidationException(string message)
        : base(message)
    {
        Errors = new List<string> { message };
    }

    public ModelValidationException(IEnumerable<string> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }

    public ModelValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = new List<string> { message };
    }
}