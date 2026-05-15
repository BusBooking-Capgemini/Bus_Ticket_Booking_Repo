using System.Text.Json;

namespace BusTicketBooking.Mvc.Models;

public sealed class ApiResult<T>
{
    public bool Succeeded { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }

    public static ApiResult<T> Success(T? data, string? message = null) => new()
    {
        Succeeded = true,
        Data = data,
        Message = message
    };

    public static ApiResult<T> Failure(string message) => new()
    {
        Succeeded = false,
        Message = message
    };
}

public sealed class ApiJsonViewModel
{
    public required string Title { get; init; }
    public required string ControllerName { get; init; }
    public string? CreateAction { get; init; }
    public ApiResult<JsonElement> Result { get; init; } = ApiResult<JsonElement>.Success(default);
}
