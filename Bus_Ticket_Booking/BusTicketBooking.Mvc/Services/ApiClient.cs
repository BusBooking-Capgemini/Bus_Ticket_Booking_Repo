using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BusTicketBooking.Mvc.Helpers;
using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;

namespace BusTicketBooking.Mvc.Services;

public sealed class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<ApiResult<JsonElement>> GetAsync(string endpoint, CancellationToken cancellationToken = default) =>
        SendAsync<object>(HttpMethod.Get, endpoint, null, cancellationToken);

    public Task<ApiResult<JsonElement>> PostAsync<TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken = default) =>
        SendAsync(HttpMethod.Post, endpoint, request, cancellationToken);

    public Task<ApiResult<JsonElement>> PutAsync<TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken = default) =>
        SendAsync(HttpMethod.Put, endpoint, request, cancellationToken);

    public Task<ApiResult<JsonElement>> PatchAsync<TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken = default) =>
        SendAsync(HttpMethod.Patch, endpoint, request, cancellationToken);

    public Task<ApiResult<JsonElement>> DeleteAsync(string endpoint, CancellationToken cancellationToken = default) =>
        SendAsync<object>(HttpMethod.Delete, endpoint, null, cancellationToken);

    private async Task<ApiResult<JsonElement>> SendAsync<TRequest>(HttpMethod method, string endpoint, TRequest? request, CancellationToken cancellationToken)
    {
        try
        {
            using var message = new HttpRequestMessage(method, endpoint);
            var token = _httpContextAccessor.HttpContext?.Session.GetString(SessionKeys.Token);

            if (!string.IsNullOrWhiteSpace(token))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (request is not null)
            {
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            using var response = await _httpClient.SendAsync(message, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var payload = ParseJson(content);

            if (!response.IsSuccessStatusCode)
            {
                var detail = string.IsNullOrWhiteSpace(content) ? response.ReasonPhrase : JsonDisplayHelper.ToDisplay(payload);
                return ApiResult<JsonElement>.Failure($"API returned {(int)response.StatusCode}: {detail}");
            }

            return ApiResult<JsonElement>.Success(payload, string.IsNullOrWhiteSpace(content) ? "Request completed." : null);
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<JsonElement>.Failure($"Could not reach the API. {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return ApiResult<JsonElement>.Failure("The API request timed out.");
        }
    }

    private static JsonElement ParseJson(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return JsonDocument.Parse("{}").RootElement.Clone();
        }

        try
        {
            return JsonDocument.Parse(content).RootElement.Clone();
        }
        catch (JsonException)
        {
            return JsonDocument.Parse(JsonSerializer.Serialize(new { message = content })).RootElement.Clone();
        }
    }
}
