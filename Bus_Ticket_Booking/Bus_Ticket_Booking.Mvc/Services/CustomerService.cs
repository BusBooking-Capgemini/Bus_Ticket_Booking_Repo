using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Customer;
using Newtonsoft.Json;
using System.Text;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class CustomerService
        : ICustomerService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        private readonly string _baseUrl;

        public CustomerService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _configuration = configuration;

            _baseUrl =
                _configuration[
                    "ApiSettings:BaseUrl"]
                ?? "";
        }

        // GET PROFILE

        public async Task<CustomerProfileViewModel?>
            GetProfileAsync(
                string token)
        {
            _httpClient.DefaultRequestHeaders
                .Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.GetAsync(
                    $"{_baseUrl}customers/getCustomer");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            return JsonConvert.DeserializeObject
                <CustomerProfileViewModel>(
                    content);
        }

        // UPDATE PROFILE

        public async Task<bool>
            UpdateProfileAsync(
                CustomerProfileViewModel model,
                string token)
        {
            _httpClient.DefaultRequestHeaders
                .Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var request = new
            {
                model.Name,
                model.Email,
                model.Phone,
                model.Address,
                model.City,
                model.State,
                model.ZipCode
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
                await _httpClient.PatchAsync(
                    $"{_baseUrl}customers/updateCustomer",
                    content);

            return response.IsSuccessStatusCode;
        }
    }
}