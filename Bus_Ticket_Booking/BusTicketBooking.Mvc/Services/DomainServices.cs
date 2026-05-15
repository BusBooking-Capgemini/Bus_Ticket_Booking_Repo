using System.Text.Json;
using BusTicketBooking.Mvc.Interfaces;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;

namespace BusTicketBooking.Mvc.Services;

public sealed class AuthService(IApiClient apiClient) : IAuthService
{
    public Task<ApiResult<JsonElement>> LoginAsync(LoginViewModel model)
    {
        var endpoint = model.Role switch
        {
            "Agency" => "api/auth/agency-login",
            "Office" => "api/auth/office-login",
            _ => "api/auth/customer-login"
        };

        return apiClient.PostAsync(endpoint, new { model.Email, model.Password });
    }

    public Task<ApiResult<JsonElement>> RegisterCustomerAsync(CustomerSignupViewModel model) =>
        apiClient.PostAsync("api/auth/customer-signup", model);
}

public sealed class AgencyService(IApiClient apiClient) : IAgencyService
{
    public Task<ApiResult<JsonElement>> GetAllAsync() => apiClient.GetAsync("api/agencies");
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/agencies/{id}");
    public Task<ApiResult<JsonElement>> UpdateAsync(int id, AgencyRequestViewModel model) => apiClient.PutAsync($"api/agencies/{id}", model);
    public Task<ApiResult<JsonElement>> DeleteAsync(int id) => apiClient.DeleteAsync($"api/agencies/{id}");
    public Task<ApiResult<JsonElement>> GetOfficesAsync(int id) => apiClient.GetAsync($"api/agencies/{id}/offices");
    public Task<ApiResult<JsonElement>> GetSummaryAsync(int id) => apiClient.GetAsync($"api/agencies/{id}/summary");
}

public sealed class OfficeService(IApiClient apiClient) : IOfficeService
{
    public Task<ApiResult<JsonElement>> GetAllAsync() => apiClient.GetAsync("api/offices");
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/offices/{id}");
    public Task<ApiResult<JsonElement>> CreateAsync(OfficeRequestViewModel model) => apiClient.PostAsync("api/offices", model);
    public Task<ApiResult<JsonElement>> UpdateAsync(int id, OfficeRequestViewModel model) => apiClient.PutAsync($"api/offices/{id}", model);
    public Task<ApiResult<JsonElement>> DeleteAsync(int id) => apiClient.DeleteAsync($"api/offices/{id}");
    public Task<ApiResult<JsonElement>> GetSummaryAsync(int id) => apiClient.GetAsync($"api/offices/{id}/summary");
}

public sealed class BusService(IApiClient apiClient) : IBusService
{
    public Task<ApiResult<JsonElement>> GetAllAsync() => apiClient.GetAsync("api/buses");
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/buses/{id}");
    public Task<ApiResult<JsonElement>> CreateAsync(BusRequestViewModel model) => apiClient.PostAsync("api/buses", model);
    public Task<ApiResult<JsonElement>> UpdateAsync(int id, BusRequestViewModel model) => apiClient.PutAsync($"api/buses/{id}", model);
    public Task<ApiResult<JsonElement>> DeleteAsync(int id) => apiClient.DeleteAsync($"api/buses/{id}");
}

public sealed class DriverService(IApiClient apiClient) : IDriverService
{
    public Task<ApiResult<JsonElement>> GetAllAsync() => apiClient.GetAsync("api/drivers");
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/drivers/{id}");
    public Task<ApiResult<JsonElement>> CreateAsync(DriverRequestViewModel model) => apiClient.PostAsync("api/drivers", model);
    public Task<ApiResult<JsonElement>> UpdateAsync(int id, DriverRequestViewModel model) => apiClient.PutAsync($"api/drivers/{id}", model);
    public Task<ApiResult<JsonElement>> DeleteAsync(int id) => apiClient.DeleteAsync($"api/drivers/{id}");
}

public sealed class RouteService(IApiClient apiClient) : IRouteService
{
    public Task<ApiResult<JsonElement>> GetAllAsync() => apiClient.GetAsync("api/routes");
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/routes/{id}");
    public Task<ApiResult<JsonElement>> CreateAsync(CreateRouteViewModel model) => apiClient.PostAsync("api/routes", model);
    public Task<ApiResult<JsonElement>> UpdateAsync(int id, CreateRouteViewModel model) => apiClient.PutAsync($"api/routes/{id}", model);
    public Task<ApiResult<JsonElement>> DeleteAsync(int id) => apiClient.DeleteAsync($"api/routes/{id}");
}

public sealed class TripService(IApiClient apiClient) : ITripService
{
    public Task<ApiResult<JsonElement>> GetAllAsync() => apiClient.GetAsync("api/trips");
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/trips/{id}");
    public Task<ApiResult<JsonElement>> CreateAsync(CreateTripViewModel model) => apiClient.PostAsync("api/trips", model);
    public Task<ApiResult<JsonElement>> UpdateAsync(int id, CreateTripViewModel model) => apiClient.PutAsync($"api/trips/{id}", model);
    public Task<ApiResult<JsonElement>> DeleteAsync(int id) => apiClient.DeleteAsync($"api/trips/{id}");
    public Task<ApiResult<JsonElement>> SearchAsync(string fromCity, string toCity, DateTime tripDate) =>
        apiClient.GetAsync($"api/trips/search?FromCity={Uri.EscapeDataString(fromCity)}&ToCity={Uri.EscapeDataString(toCity)}&TripDate={Uri.EscapeDataString(tripDate.ToString("O"))}");
}

public sealed class BookingService(IApiClient apiClient) : IBookingService
{
    public Task<ApiResult<JsonElement>> CreateAsync(CreateBookingViewModel model) => apiClient.PostAsync("api/booking/create", model);
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/booking/get-by-id/{id}");
    public Task<ApiResult<JsonElement>> CancelAsync(int id) => apiClient.PutAsync($"api/booking/cancel/{id}", new { });
    public Task<ApiResult<JsonElement>> DashboardAsync(int? agencyId, int? officeId) => apiClient.GetAsync(Query("api/booking/dashboard", agencyId, officeId));

    private static string Query(string endpoint, int? agencyId, int? officeId) =>
        $"{endpoint}?agencyId={agencyId?.ToString() ?? string.Empty}&officeId={officeId?.ToString() ?? string.Empty}";
}

public sealed class PaymentService(IApiClient apiClient) : IPaymentService
{
    public Task<ApiResult<JsonElement>> CreateAsync(CreatePaymentViewModel model) => apiClient.PostAsync("api/payment/create", model);
    public Task<ApiResult<JsonElement>> GetAsync(int id) => apiClient.GetAsync($"api/payment/get-by-id/{id}");
    public Task<ApiResult<JsonElement>> DashboardAsync(int? agencyId, int? officeId) => apiClient.GetAsync(Query("api/payment/dashboard", agencyId, officeId));
    public Task<ApiResult<JsonElement>> RevenueSummaryAsync(int? agencyId, int? officeId) => apiClient.GetAsync(Query("api/payment/revenue-summary", agencyId, officeId));

    private static string Query(string endpoint, int? agencyId, int? officeId) =>
        $"{endpoint}?agencyId={agencyId?.ToString() ?? string.Empty}&officeId={officeId?.ToString() ?? string.Empty}";
}

public sealed class CustomerService(IApiClient apiClient) : ICustomerService
{
    public Task<ApiResult<JsonElement>> RegisterAsync(CustomerRequestViewModel model) => apiClient.PostAsync("api/customers/register", model);
    public Task<ApiResult<JsonElement>> GetCurrentAsync() => apiClient.GetAsync("api/customers/getCustomer");
    public Task<ApiResult<JsonElement>> UpdateCurrentAsync(CustomerRequestViewModel model) => apiClient.PatchAsync("api/customers/updateCustomer", model);
    public Task<ApiResult<JsonElement>> DeleteCurrentAsync() => apiClient.DeleteAsync("api/customers/deleteCustomer");
}
