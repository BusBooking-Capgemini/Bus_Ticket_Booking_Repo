using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.OfficeDashboard;
using Newtonsoft.Json;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class OfficeDashboardService
        : IOfficeDashboardService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        private readonly string
            _baseUrl;

        public OfficeDashboardService(
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

        public async Task<
            OfficeDashboardViewModel?>
            GetDashboardAsync(
                int officeId,
                string token)
        {
            _httpClient
                .DefaultRequestHeaders
                .Authorization =
                    new System.Net.Http.Headers
                    .AuthenticationHeaderValue(
                        "Bearer",
                        token);

            var model =
                new OfficeDashboardViewModel();


            var officeResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}offices/{officeId}/summary");

            if (officeResponse.IsSuccessStatusCode)
            {
                var content =
                    await officeResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content);

                model.OfficeId =
                    result.data.officeId;

                model.TotalBuses =
                    result.data.totalBuses;

                model.TotalDrivers =
                    result.data.totalDrivers;
            }

            

            var bookingDashboardResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}booking/dashboard?officeId={officeId}");

            if (bookingDashboardResponse.IsSuccessStatusCode)
            {
                var content =
                    await bookingDashboardResponse
                        .Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content);

                model.TotalBookings =
                    result.data.totalBookings;

                model.ActiveBookings =
                    result.data.activeBookings;
            }


            var bookingAnalyticsResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}booking/analytics?officeId={officeId}");

            if (bookingAnalyticsResponse.IsSuccessStatusCode)
            {
                var content =
                    await bookingAnalyticsResponse
                        .Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content);

                model.OccupancyRate =
                    result.data.occupancyRate;

                model.MostBookedTripId =
                    result.data.mostBookedTripId;

                model.MostPopularRoute =
                    result.data.mostPopularRoute;
            }

           

            var paymentDashboardResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}payment/dashboard?officeId={officeId}");

            if (paymentDashboardResponse.IsSuccessStatusCode)
            {
                var content =
                    await paymentDashboardResponse
                        .Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content);

                model.TotalPayments =
                    result.data.totalPayments;

                model.SuccessfulPayments =
                    result.data.successfulPayments;

                model.FailedPayments =
                    result.data.failedPayments;

                model.TotalRevenue =
                    result.data.totalRevenue;
            }


            var paymentAnalyticsResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}payment/analytics?officeId={officeId}");

            if (paymentAnalyticsResponse.IsSuccessStatusCode)
            {
                var content =
                    await paymentAnalyticsResponse
                        .Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content);

                model.SuccessRate =
                    result.data.successRate;

                model.FailureRate =
                    result.data.failureRate;

                model.AveragePaymentAmount =
                    result.data.averagePaymentAmount;

                model.TopPayingRoute =
                    result.data.topPayingRoute;
            }

            return model;
        }
    }
}