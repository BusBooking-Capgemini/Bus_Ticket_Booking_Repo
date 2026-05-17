using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Bus;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class BusService : IBusService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        private readonly string
            _baseUrl;

        public BusService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient =
                httpClient;

            _configuration =
                configuration;

            _baseUrl =
                _configuration["ApiSettings:BaseUrl"]
                ?? "";
        }

        // =========================
        // GET ALL
        // =========================

        public async Task<List<BusViewModel>>
    GetOfficeBusesAsync(
        string token)
        {
            var officeId =
                JwtHelper.GetClaim(
                    token,
                    "OfficeId");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}buses/office/{officeId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<BusViewModel>();
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json)!;

            return JsonConvert.DeserializeObject<
                List<BusViewModel>>(
                    result.data.ToString())
                ?? new List<BusViewModel>();
        }

        // =========================
        // GET BY ID
        // =========================

        public async Task<BusViewModel?>
    GetBusByIdAsync(
        int id,
        string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}buses/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<BusViewModel>>(
                        content);

            return result?.Data;
        }

        // =========================
        // CREATE
        // =========================

        public async Task<bool>
            CreateBusAsync(
                CreateBusViewModel model,
                string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var json =
                JsonConvert.SerializeObject(
                    model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    $"{_baseUrl}buses",
                    content);

            return response.IsSuccessStatusCode;
        }

        // =========================
        // UPDATE
        // =========================

        public async Task<bool>
    UpdateBusAsync(
        UpdateBusViewModel model,
        string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var request = new
            {
                OfficeId =
                    model.OfficeId,

                RegistrationNumber =
                    model.RegistrationNumber,

                Capacity =
                    model.Capacity,

                Type =
                    model.Type
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
                    $"{_baseUrl}buses/{model.BusId}",
                    content);

            return response.IsSuccessStatusCode;
        }

        // =========================
        // DELETE
        // =========================

        public async Task<bool>
            DeleteBusAsync(
                int id,
                string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.DeleteAsync(
                    $"{_baseUrl}buses/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}