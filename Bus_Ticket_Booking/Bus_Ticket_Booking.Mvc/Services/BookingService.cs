using System.Text;
using Bus_Ticket_Booking.Mvc.Models.Booking;
using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Booking;
using Newtonsoft.Json;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        private readonly string _baseUrl;

        public BookingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            _configuration = configuration;

            _baseUrl = _configuration["ApiSettings:BaseUrl"] ?? "";
        }

        // CREATE BOOKING

        public async Task<BookingResponseViewModel?> CreateBookingAsync(
            int tripId,
            int seatNumber,
            string token
        )
        {
            var request = new CreateBookingRequestModel
            {
                TripId = tripId,
                SeatNumber = seatNumber,
            };

            var json = JsonConvert.SerializeObject(request);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"{_baseUrl}booking/create", content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<BookingResponseViewModel>>(
                responseContent
            );

            return result?.Data;
        }

        // MY BOOKINGS

        public async Task<List<BookingListViewModel>> GetMyBookingsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}booking/my-bookings");

            if (!response.IsSuccessStatusCode)
            {
                return new List<BookingListViewModel>();
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<List<BookingListViewModel>>>(
                content
            );

            return result?.Data ?? new List<BookingListViewModel>();
        }

        public async Task<bool> CancelBookingAsync(int bookingId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync(
                $"{_baseUrl}booking/cancel/{bookingId}",
                null
            );

            return response.IsSuccessStatusCode;
        }
    }
}
