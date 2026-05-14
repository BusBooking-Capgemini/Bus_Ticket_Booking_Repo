using API_Bus_Ticket_Booking.DTOs.Payment;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class PaymentTestData
    {
        public static CreatePaymentDto GetCreatePaymentDto()
        {
            return new CreatePaymentDto
            {
                BookingId = 1,
                CustomerId = 1,
                Amount = 250.50m
            };
        }

        public static PaymentResponseDto GetPaymentResponse()
        {
            return new PaymentResponseDto
            {
                PaymentId = 1,
                BookingId = 1,
                Amount = 250.50m,
                PaymentStatus = "Success",
                PaymentDate = DateTime.UtcNow
            };
        }

        public static List<PaymentResponseDto> GetPayments()
        {
            return new List<PaymentResponseDto>
            {
                new PaymentResponseDto
                {
                    PaymentId = 1,
                    BookingId = 1,
                    Amount = 250.50m,
                    PaymentStatus = "Success",
                    PaymentDate = DateTime.UtcNow
                },
                new PaymentResponseDto
                {
                    PaymentId = 2,
                    BookingId = 2,
                    Amount = 300.00m,
                    PaymentStatus = "Failed",
                    PaymentDate = DateTime.UtcNow
                }
            };
        }

        public static RevenueSummaryDto GetRevenueSummary()
        {
            return new RevenueSummaryDto
            {
                TotalRevenue = 10000m,
                TodayRevenue = 500m,
                MonthlyRevenue = 2000m
            };
        }

        public static PaymentDashboardDto GetDashboard()
        {
            return new PaymentDashboardDto
            {
                TotalPayments = 100,
                SuccessfulPayments = 90,
                FailedPayments = 10,
                TotalRevenue = 25000m
            };
        }

        public static PaymentAnalyticsDto GetAnalytics()
        {
            return new PaymentAnalyticsDto
            {
                SuccessRate = 90.0,
                FailureRate = 10.0,
                AveragePaymentAmount = 250.5m,
                TopPayingRoute = "CityA to CityB"
            };
        }
    }
}
