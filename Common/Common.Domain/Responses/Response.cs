namespace Common.Domain.Responses;

public class Response<T>
{
    public T? Data { get; }
    public bool Success { get; }
    public string? Message { get; }
    public string[]? Errors { get; }

    protected Response(T? data, bool success, string? message = null, string[]? errors = null)
    {
        Data = data;
        Success = success;
        Message = message;
        Errors = errors;
    }

    public static Response<T> Succeed(T data, string? message = null)
    {
        return new Response<T>(data, true, message);
    }

    public static Response<T> Fail(string? message = null, string[]? errors = null)
    {
        return new Response<T>(default, false, message, errors);
    }
}
