using System.Text.Json;
using BusTicketBooking.Mvc.Models;

namespace BusTicketBooking.Mvc.Interfaces;

public interface IApiClient
{
    Task<ApiResult<JsonElement>> GetAsync(string endpoint, CancellationToken cancellationToken = default);
    Task<ApiResult<JsonElement>> PostAsync<TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
    Task<ApiResult<JsonElement>> PutAsync<TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
    Task<ApiResult<JsonElement>> PatchAsync<TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
    Task<ApiResult<JsonElement>> DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
}
