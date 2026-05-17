using Bus_Ticket_Booking.Mvc.Models.Common;
using Bus_Ticket_Booking.Mvc.Services.Interfaces;
using Bus_Ticket_Booking.Mvc.ViewModels.AgencyDashboard;
using Newtonsoft.Json;

namespace Bus_Ticket_Booking.Mvc.Services
{
    public class AgencyDashboardService
        : IAgencyDashboardService
    {
        private readonly HttpClient
            _httpClient;

        private readonly IConfiguration
            _configuration;

        private readonly string
            _baseUrl;

        public AgencyDashboardService(
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
        // DASHBOARD
        // =========================

        public async Task<
            AgencyDashboardViewModel?>
            GetDashboardAsync(
                int agencyId,
                string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var model =
                new AgencyDashboardViewModel();

            // =========================
            // AGENCY INFO
            // =========================

            var agencyResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}");

            if (agencyResponse.IsSuccessStatusCode)
            {
                var content =
                    await agencyResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.AgencyId =
                        result.data.agencyId;

                    model.Name =
                        result.data.name;

                    model.ContactPersonName =
                        result.data.contactPersonName;

                    model.Email =
                        result.data.email;

                    model.Phone =
                        result.data.phone;
                }
            }

            // =========================
            // SUMMARY
            // =========================

            var summaryResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}/summary");

            if (summaryResponse.IsSuccessStatusCode)
            {
                var content =
                    await summaryResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.TotalOffices =
                        result.data.totalOffices;
                }
            }

            // =========================
            // OFFICES
            // =========================

            var officesResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}/offices");

            if (officesResponse.IsSuccessStatusCode)
            {
                var content =
                    await officesResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.Offices =
                        JsonConvert.DeserializeObject
                        <List<AgencyOfficeCardViewModel>>(
                            result.data.ToString())
                        ?? new List<AgencyOfficeCardViewModel>();
                }
            }

            // =========================
            // BOOKING DASHBOARD
            // =========================

            var bookingDashboardResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}booking/dashboard?agencyId={agencyId}");

            if (bookingDashboardResponse.IsSuccessStatusCode)
            {
                var content =
                    await bookingDashboardResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.TotalBookings =
                        result.data.totalBookings;

                    model.ActiveBookings =
                        result.data.activeBookings;
                }
            }

            // =========================
            // BOOKING ANALYTICS
            // =========================

            var bookingAnalyticsResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}booking/analytics?agencyId={agencyId}");

            if (bookingAnalyticsResponse.IsSuccessStatusCode)
            {
                var content =
                    await bookingAnalyticsResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.OccupancyRate =
                        result.data.occupancyRate;

                    model.MostBookedTripId =
                        result.data.mostBookedTripId;

                    model.MostPopularRoute =
                        result.data.mostPopularRoute;
                }
            }

            // =========================
            // PAYMENT DASHBOARD
            // =========================

            var paymentDashboardResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}payment/dashboard?agencyId={agencyId}");

            if (paymentDashboardResponse.IsSuccessStatusCode)
            {
                var content =
                    await paymentDashboardResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.TotalPayments =
                        result.data.totalPayments;

                    model.SuccessfulPayments =
                        result.data.successfulPayments;

                    model.FailedPayments =
                        result.data.failedPayments;

                    model.TotalRevenue =
                        result.data.totalRevenue;
                }
            }

            // =========================
            // PAYMENT ANALYTICS
            // =========================

            var paymentAnalyticsResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}payment/analytics?agencyId={agencyId}");

            if (paymentAnalyticsResponse.IsSuccessStatusCode)
            {
                var content =
                    await paymentAnalyticsResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.SuccessRate =
                        result.data.successRate;

                    model.FailureRate =
                        result.data.failureRate;

                    model.AveragePaymentAmount =
                        result.data.averagePaymentAmount;

                    model.TopPayingRoute =
                        result.data.topPayingRoute;
                }
            }

            return model;
        }

        // =========================
        // OFFICE DETAILS
        // =========================

        public async Task<
    AgencyOfficeDetailsViewModel?>
    GetOfficeDetailsAsync(
        int agencyId,
        int officeId,
        string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var model =
                new AgencyOfficeDetailsViewModel();

            // =========================
            // OFFICE INFO
            // =========================

            var officeResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}offices/{officeId}");

            if (officeResponse.IsSuccessStatusCode)
            {
                var content =
                    await officeResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.OfficeId =
                        result.data.officeId;

                    model.AgencyId =
                        result.data.agencyId;

                    model.OfficeMail =
                        result.data.officeMail;

                    model.OfficeContactPersonName =
                        result.data.officeContactPersonName;

                    model.OfficeContactNumber =
                        result.data.officeContactNumber;

                    model.OfficeAddressId =
                        result.data.officeAddressId;
                }
            }

            // =========================
            // TRIPS
            // =========================

            var tripsResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}/offices/{officeId}/trips");

            if (tripsResponse.IsSuccessStatusCode)
            {
                var content =
                    await tripsResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.Trips =
                        JsonConvert.DeserializeObject
                        <List<AgencyTripViewModel>>(
                            result.data.ToString())
                        ?? new List<AgencyTripViewModel>();
                }
            }

            // =========================
            // BOOKINGS
            // =========================

            var bookingsResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}/offices/{officeId}/bookings");

            if (bookingsResponse.IsSuccessStatusCode)
            {
                var content =
                    await bookingsResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.Bookings =
                        JsonConvert.DeserializeObject
                        <List<AgencyBookingViewModel>>(
                            result.data.ToString())
                        ?? new List<AgencyBookingViewModel>();
                }
            }

            // =========================
            // PAYMENTS
            // =========================

            var paymentsResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}/offices/{officeId}/payments");

            if (paymentsResponse.IsSuccessStatusCode)
            {
                var content =
                    await paymentsResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.Payments =
                        JsonConvert.DeserializeObject
                        <List<AgencyPaymentViewModel>>(
                            result.data.ToString())
                        ?? new List<AgencyPaymentViewModel>();
                }
            }

            // =========================
            // BUSES
            // =========================

            var busesResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}/offices/{officeId}/buses");

            if (busesResponse.IsSuccessStatusCode)
            {
                var content =
                    await busesResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.Buses =
                        JsonConvert.DeserializeObject
                        <List<AgencyBusViewModel>>(
                            result.data.ToString())
                        ?? new List<AgencyBusViewModel>();
                }
            }

            // =========================
            // DRIVERS
            // =========================

            var driversResponse =
                await _httpClient.GetAsync(
                    $"{_baseUrl}agencies/{agencyId}/offices/{officeId}/drivers");

            if (driversResponse.IsSuccessStatusCode)
            {
                var content =
                    await driversResponse.Content
                        .ReadAsStringAsync();

                dynamic result =
                    JsonConvert.DeserializeObject(content)!;

                if (result?.data != null)
                {
                    model.Drivers =
                        JsonConvert.DeserializeObject
                        <List<AgencyDriverViewModel>>(
                            result.data.ToString())
                        ?? new List<AgencyDriverViewModel>();
                }
            }

            return model;
        }
    }
}