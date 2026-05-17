namespace API_Bus_Ticket_Booking.DTOs.Payment
{
    public class CreatePaymentDto
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public decimal Amount { get; set; }
    }

    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public decimal? Amount { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public string FromCity { get; set; }
            = string.Empty;

        public string ToCity { get; set; }
            = string.Empty;

        public DateTime TripDate { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }
    }

    public class RevenueSummaryDto
    {
        public decimal TotalRevenue { get; set; }

        public decimal TodayRevenue { get; set; }

        public decimal MonthlyRevenue { get; set; }
    }

    public class PaymentDashboardDto
    {
        public int TotalPayments { get; set; }

        public int SuccessfulPayments { get; set; }

        public int FailedPayments { get; set; }

        public decimal TotalRevenue { get; set; }
    }

    public class PaymentAnalyticsDto
    {
        public double SuccessRate { get; set; }

        public double FailureRate { get; set; }

        public decimal AveragePaymentAmount { get; set; }

        public string TopPayingRoute { get; set; }
            = string.Empty;
    }
}