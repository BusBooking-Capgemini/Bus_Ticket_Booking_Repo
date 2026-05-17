using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Payment;
using Newtonsoft.Json;
using System.Text;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class PaymentService
        : IPaymentService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        private readonly string _baseUrl;

        public PaymentService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _configuration = configuration;

            _baseUrl =
                _configuration["ApiSettings:BaseUrl"]
                ?? "";
        }

        // SIMULATED PAYMENT

        public async Task<bool>
            ProcessPaymentAsync(
                PaymentViewModel model)
        {
            await Task.Delay(1500);

            return true;
        }

        // CREATE PAYMENT

        public async Task<bool>
            CreatePaymentAsync(
                int bookingId,
                decimal amount,
                string token)
        {
            var customerId =
                JwtHelper.GetClaim(
                    token,
                    "CustomerId");

            var request = new
            {
                BookingId = bookingId,

                CustomerId =
                    Convert.ToInt32(
                        customerId),

                Amount = amount
            };

            var json =
                JsonConvert.SerializeObject(
                    request);

            var content =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            _httpClient.DefaultRequestHeaders
                .Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await _httpClient.PostAsync(
                    $"{_baseUrl}payment/create",
                    content);

            return response
                .IsSuccessStatusCode;
        }

        // MY PAYMENTS

        public async Task<
            List<PaymentListViewModel>>
            GetMyPaymentsAsync(
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
                    $"{_baseUrl}payment/my-payments");

            if (!response.IsSuccessStatusCode)
            {
                return new List<
                    PaymentListViewModel>();
            }

            var content =
                await response.Content
                    .ReadAsStringAsync();

            var result =
                JsonConvert.DeserializeObject<
                    ApiResponse<
                        List<PaymentListViewModel>>>
                    (content);

            return result?.Data
                   ?? new List<
                        PaymentListViewModel>();
        }
    }
}