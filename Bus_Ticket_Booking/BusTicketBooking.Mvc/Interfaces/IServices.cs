using System.Text.Json;
using BusTicketBooking.Mvc.Models;
using BusTicketBooking.Mvc.ViewModels;

namespace BusTicketBooking.Mvc.Interfaces;

public interface IAuthService
{
    Task<ApiResult<JsonElement>> LoginAsync(LoginViewModel model);
    Task<ApiResult<JsonElement>> RegisterCustomerAsync(CustomerSignupViewModel model);
}

public interface IAgencyService
{
    Task<ApiResult<JsonElement>> GetAllAsync();
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> UpdateAsync(int id, AgencyRequestViewModel model);
    Task<ApiResult<JsonElement>> DeleteAsync(int id);
    Task<ApiResult<JsonElement>> GetOfficesAsync(int id);
    Task<ApiResult<JsonElement>> GetSummaryAsync(int id);
}

public interface IOfficeService
{
    Task<ApiResult<JsonElement>> GetAllAsync();
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> CreateAsync(OfficeRequestViewModel model);
    Task<ApiResult<JsonElement>> UpdateAsync(int id, OfficeRequestViewModel model);
    Task<ApiResult<JsonElement>> DeleteAsync(int id);
    Task<ApiResult<JsonElement>> GetSummaryAsync(int id);
}

public interface IBusService
{
    Task<ApiResult<JsonElement>> GetAllAsync();
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> CreateAsync(BusRequestViewModel model);
    Task<ApiResult<JsonElement>> UpdateAsync(int id, BusRequestViewModel model);
    Task<ApiResult<JsonElement>> DeleteAsync(int id);
}

public interface IDriverService
{
    Task<ApiResult<JsonElement>> GetAllAsync();
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> CreateAsync(DriverRequestViewModel model);
    Task<ApiResult<JsonElement>> UpdateAsync(int id, DriverRequestViewModel model);
    Task<ApiResult<JsonElement>> DeleteAsync(int id);
}

public interface IRouteService
{
    Task<ApiResult<JsonElement>> GetAllAsync();
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> CreateAsync(CreateRouteViewModel model);
    Task<ApiResult<JsonElement>> UpdateAsync(int id, CreateRouteViewModel model);
    Task<ApiResult<JsonElement>> DeleteAsync(int id);
}

public interface ITripService
{
    Task<ApiResult<JsonElement>> GetAllAsync();
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> CreateAsync(CreateTripViewModel model);
    Task<ApiResult<JsonElement>> UpdateAsync(int id, CreateTripViewModel model);
    Task<ApiResult<JsonElement>> DeleteAsync(int id);
    Task<ApiResult<JsonElement>> SearchAsync(string fromCity, string toCity, DateTime tripDate);
}

public interface IBookingService
{
    Task<ApiResult<JsonElement>> CreateAsync(CreateBookingViewModel model);
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> CancelAsync(int id);
    Task<ApiResult<JsonElement>> DashboardAsync(int? agencyId, int? officeId);
}

public interface IPaymentService
{
    Task<ApiResult<JsonElement>> CreateAsync(CreatePaymentViewModel model);
    Task<ApiResult<JsonElement>> GetAsync(int id);
    Task<ApiResult<JsonElement>> DashboardAsync(int? agencyId, int? officeId);
    Task<ApiResult<JsonElement>> RevenueSummaryAsync(int? agencyId, int? officeId);
}

public interface ICustomerService
{
    Task<ApiResult<JsonElement>> RegisterAsync(CustomerRequestViewModel model);
    Task<ApiResult<JsonElement>> GetCurrentAsync();
    Task<ApiResult<JsonElement>> UpdateCurrentAsync(CustomerRequestViewModel model);
    Task<ApiResult<JsonElement>> DeleteCurrentAsync();
}
