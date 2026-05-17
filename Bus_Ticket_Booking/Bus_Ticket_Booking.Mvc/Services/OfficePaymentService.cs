using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.OfficePayment;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class OfficePaymentService
        : IOfficePaymentService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        public OfficePaymentService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient =
                httpClient;

            _configuration =
                configuration;

            _httpClient.BaseAddress =
                new Uri(
                    _configuration["ApiSettings:BaseUrl"]!);
        }

        public async Task<List<OfficePaymentViewModel>>
            GetOfficePaymentsAsync(
                string token)
        {
            try
            {
                var officeId =
                    JwtHelper.GetOfficeId(
                        token);

                if (string.IsNullOrEmpty(
                    officeId))
                {
                    return new List<OfficePaymentViewModel>();
                }

                _httpClient
                    .DefaultRequestHeaders
                    .Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);

                var response =
                    await _httpClient.GetAsync(
                        $"payment/office-payments/{officeId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<OfficePaymentViewModel>();
                }

                var json =
                    await response.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(
                        json)!;

                if (result == null ||
                    result.data == null)
                {
                    return new List<OfficePaymentViewModel>();
                }

                return JsonConvert.DeserializeObject
                    <List<OfficePaymentViewModel>>(
                        result.data.ToString())
                    ?? new List<OfficePaymentViewModel>();
            }
            catch
            {
                return new List<OfficePaymentViewModel>();
            }
        }
    }
}