using Bus_Ticket_Booking.Mvc.Models.Auth;
using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Auth;
using Newtonsoft.Json;
using System.Text;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        private readonly string _baseUrl;

        public AuthService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _configuration = configuration;

            _baseUrl =
                _configuration["ApiSettings:BaseUrl"]!;
        }

        public async Task<AuthResponseViewModel?>
            CustomerLoginAsync(
                LoginViewModel model)
        {
            var request =
                new LoginRequestModel
                {
                    Email = model.Email,
                    Password = model.Password
                };

            return await LoginAsync(
                "auth/customer-login",
                request);
        }

        public async Task<AuthResponseViewModel?>
    AgencyLoginAsync(
        AgencyLoginViewModel model)
        {
            var json =
                JsonConvert.SerializeObject(model);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    $"{_baseUrl}auth/agency-login",
                    content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<AuthResponseViewModel>>(
                        responseContent);

            return result?.Data;
        }

        public async Task<AuthResponseViewModel?>
            OfficeLoginAsync(
                LoginViewModel model)
        {
            var request =
                new LoginRequestModel
                {
                    Email = model.Email,
                    Password = model.Password
                };

            return await LoginAsync(
                "auth/office-login",
                request);
        }

        public async Task<bool>
            CustomerSignupAsync(
                RegisterViewModel model)
        {
            var request =
                new CustomerSignupRequestModel
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password
                };

            var json =
                JsonConvert.SerializeObject(request);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    $"{_baseUrl}auth/customer-signup",
                    content);

            return response.IsSuccessStatusCode;
        }

        private async Task<AuthResponseViewModel?>
            LoginAsync(
                string endpoint,
                LoginRequestModel request)
        {
            var json =
                JsonConvert.SerializeObject(request);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            var response =
                await _httpClient.PostAsync(
                    $"{_baseUrl}{endpoint}",
                    content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject
                <ApiResponse<AuthResponseViewModel>>
                (responseContent);

            return result?.Data;
        }
    }
}