using Bus_Ticket_Booking.Mvc.Helpers;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.OfficeBooking;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class OfficeBookingService
        : IOfficeBookingService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        public OfficeBookingService(
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

        public async Task<List<OfficeBookingViewModel>>
            GetOfficeBookingsAsync(
                string token)
        {
            try
            {
                var officeId =
                    JwtHelper.GetOfficeId(token);

                if (string.IsNullOrEmpty(officeId))
                {
                    return new List<OfficeBookingViewModel>();
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);

                var response =
                    await _httpClient.GetAsync(
                        $"booking/office-bookings/{officeId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<OfficeBookingViewModel>();
                }

                var json =
                    await response.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(json)!;

                if (result == null ||
                    result.data == null)
                {
                    return new List<OfficeBookingViewModel>();
                }

                return JsonConvert.DeserializeObject
                    <List<OfficeBookingViewModel>>(
                        result.data.ToString())
                    ?? new List<OfficeBookingViewModel>();
            }
            catch
            {
                return new List<OfficeBookingViewModel>();
            }
        }
    }
}