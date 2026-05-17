using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Driver;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class DriverService : IDriverService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        private readonly string
            _baseUrl;

        public DriverService(
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

        public async Task<List<DriverViewModel>>
    GetOfficeDriversAsync(
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
                    $"{_baseUrl}drivers/office/{officeId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<DriverViewModel>();
            }

            var json =
                await response.Content.ReadAsStringAsync();

            dynamic result =
                JsonConvert.DeserializeObject(json)!;

            return JsonConvert.DeserializeObject<
                List<DriverViewModel>>(
                    result.data.ToString())
                ?? new List<DriverViewModel>();
        }

        public async Task<DriverViewModel?>
            GetDriverByIdAsync(
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
                    $"{_baseUrl}drivers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<DriverViewModel>>(
                        content);

            return result?.Data;
        }

        public async Task<bool>
            CreateDriverAsync(
                CreateDriverViewModel model,
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
                    $"{_baseUrl}drivers",
                    content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool>
            UpdateDriverAsync(
                UpdateDriverViewModel model,
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
                await _httpClient.PutAsync(
                    $"{_baseUrl}drivers/{model.DriverId}",
                    content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool>
            DeleteDriverAsync(
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
                    $"{_baseUrl}drivers/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}