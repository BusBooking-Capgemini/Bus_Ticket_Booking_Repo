using System.Text;
using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Models.Review;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.Review;
using Newtonsoft.Json;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class ReviewService : IReviewService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ReviewService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "";
        }

        public async Task<List<ReviewViewModel>> GetMyReviewsAsync(int customerId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(
                $"{_baseUrl}customers/{customerId}/getReviews"
            );

            if (!response.IsSuccessStatusCode)
                return new List<ReviewViewModel>();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<ReviewViewModel>>(content);

            return result ?? new List<ReviewViewModel>();
        }

        public async Task<bool> CreateReviewAsync(
            int customerId,
            CreateReviewViewModel model,
            string token
        )
        {
            var request = new CreateReviewRequestModel
            {
                TripId = model.TripId,
                Rating = model.Rating,
                Comment = model.Comment,
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(
                $"{_baseUrl}customers/{customerId}/createReview",
                content
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteReviewAsync(int customerId, int reviewId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync(
                $"{_baseUrl}customers/{customerId}/deleteReview/{reviewId}"
            );

            return response.IsSuccessStatusCode;
        }
    }
}