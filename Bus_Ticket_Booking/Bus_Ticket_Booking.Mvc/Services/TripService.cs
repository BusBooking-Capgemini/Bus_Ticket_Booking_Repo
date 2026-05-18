using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Trip;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class TripService : ITripService
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration
            _configuration;

        private readonly string _baseUrl;

        public TripService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _configuration = configuration;

            _baseUrl =
                _configuration["ApiSettings:BaseUrl"]
                ?? "";
        }

        // =========================
        // GET ALL TRIPS
        // =========================

        public async Task<List<TripViewModel>>
            GetAllTripsAsync()
        {
            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}trips");

            if (!response.IsSuccessStatusCode)
            {
                return new List<TripViewModel>();
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<List<TripViewModel>>>
                    (content);

            return result?.Data
                   ?? new List<TripViewModel>();
        }

        // =========================
        // SEARCH TRIPS
        // =========================

        public async Task<List<TripViewModel>>
    SearchTripsAsync(
        TripSearchViewModel model)
        {
            var queryParams =
                new List<string>();

            if (!string.IsNullOrWhiteSpace(
                model.FromCity))
            {
                queryParams.Add(
                    $"fromCity={model.FromCity}");
            }

            if (!string.IsNullOrWhiteSpace(
                model.ToCity))
            {
                queryParams.Add(
                    $"toCity={model.ToCity}");
            }

            if (model.TripDate.HasValue)
            {
                queryParams.Add(
                    $"tripDate={model.TripDate.Value:yyyy-MM-dd}");
            }

            var query =
                string.Join("&", queryParams);

            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}trips/search?{query}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<TripViewModel>();
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<List<TripViewModel>>>(
                        content);

            return result?.Data
                   ?? new List<TripViewModel>();
        }

        // =========================
        // SEAT MAP
        // =========================

        public async Task<SeatMapViewModel?>
            GetSeatMapAsync(
                int tripId)
        {
            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}trips/{tripId}/seats");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<SeatMapViewModel>>
                    (content);

            return result?.Data;
        }

        // =========================
        // OFFICE TRIPS
        // =========================

        public async Task<List<TripViewModel>>
    GetOfficeTripsAsync(
        string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var officeId =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}trips/office/{officeId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<TripViewModel>();
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<List<TripViewModel>>>(
                        content);

            return result?.Data
                   ?? new List<TripViewModel>();
        }

        // =========================
        // GET TRIP BY ID
        // =========================

        public async Task<TripViewModel?>
            GetTripByIdAsync(
                int tripId)
        {
            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}trips/{tripId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<TripViewModel>>
                    (content);

            return result?.Data;
        }

        // =========================
        // CREATE TRIP
        // =========================

        public async Task<bool>
            CreateTripAsync(
                CreateTripViewModel model,
                string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var request = new
            {
                RouteId = model.RouteId,
                BusId = model.BusId,
                BoardingAddressId = model.BoardingAddressId,
                DroppingAddressId = model.DroppingAddressId,
                DepartureTime = model.DepartureTime,
                ArrivalTime = model.ArrivalTime,
                Driver1DriverId = model.Driver1DriverId,
                Driver2DriverId = model.Driver2DriverId,
                Fare = model.Fare,
                TripDate = model.TripDate
            };

            var json =
                JsonConvert.SerializeObject(
                    request);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    $"{_baseUrl}trips",
                    content);

            return response.IsSuccessStatusCode;
        }

        // =========================
        // UPDATE TRIP
        // =========================

        public async Task<bool>
            UpdateTripAsync(
                UpdateTripViewModel model,
                string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var request = new
            {
                BusId = model.BusId,

                BoardingAddressId =
        model.BoardingAddressId,

                DroppingAddressId =
        model.DroppingAddressId,

                DepartureTime =
        model.DepartureTime,

                ArrivalTime =
        model.ArrivalTime,

                Driver1DriverId =
        model.Driver1DriverId,

                Driver2DriverId =
        model.Driver2DriverId,

                Fare =
        model.Fare,

                TripDate =
        model.TripDate
            };

            var json =
                JsonConvert.SerializeObject(
                    request);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PutAsync(
                    $"{_baseUrl}trips/{model.TripId}",
                    content);

            return response.IsSuccessStatusCode;
        }

        // =========================
        // DELETE TRIP
        // =========================

        public async Task<bool>
            DeleteTripAsync(
                int tripId,
                string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.DeleteAsync(
                    $"{_baseUrl}trips/{tripId}");

            return response.IsSuccessStatusCode;
        }

        // =========================
        // ROUTES DROPDOWN
        // =========================

        public async Task<List<SelectListItem>>
            GetRoutesDropdownAsync(
                string token)
        {
            return await GetDropdownAsync(
                $"{_baseUrl}dropdowns/routes",
                token);
        }

        // =========================
        // BUSES DROPDOWN
        // =========================

        public async Task<List<SelectListItem>>
    GetBusesDropdownAsync(
        string token)
        {
            var officeId =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            return await GetDropdownAsync(
                $"{_baseUrl}dropdowns/buses/{officeId}",
                token);
        }

        // =========================
        // ADDRESSES DROPDOWN
        // =========================

        public async Task<List<SelectListItem>>
            GetAddressesDropdownAsync(
                string token)
        {
            return await GetDropdownAsync(
                $"{_baseUrl}dropdowns/addresses",
                token);
        }

        // =========================
        // DRIVERS DROPDOWN
        // =========================

        public async Task<List<SelectListItem>>
    GetDriversDropdownAsync(
        string token)
        {
            var officeId =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            return await GetDropdownAsync(
                $"{_baseUrl}dropdowns/drivers/{officeId}",
                token);
        }

        // =========================
        // COMMON DROPDOWN METHOD
        // =========================

        private async Task<List<SelectListItem>>
            GetDropdownAsync(
                string url,
                string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<SelectListItem>();
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<List<DropdownResponseModel>>>(
                        content);

            return result?.Data?
                .Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                .ToList()

                ?? new List<SelectListItem>();
        }
    }
}