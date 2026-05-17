using System.ComponentModel.DataAnnotations;

namespace API_Bus_Ticket_Booking.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int TripId { get; set; }

        public int SeatNumber { get; set; }
    }

    public class BookingResponseDto
    {
        public int BookingId { get; set; }

        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public string Status { get; set; }
            = string.Empty;

        public string FromCity { get; set; }
            = string.Empty;

        public string ToCity { get; set; }
            = string.Empty;

        public DateTime TripDate { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Fare { get; set; }

        public string PaymentStatus { get; set; }
            = string.Empty;

        public decimal PaidAmount { get; set; }
    }

    public class BookingDashboardDto
    {
        public int TotalBookings { get; set; }

        public int ActiveBookings { get; set; }
    }

    public class BookingAnalyticsDto
    {
        public double OccupancyRate { get; set; }

        public int MostBookedTripId { get; set; }

        public string MostPopularRoute { get; set; }
            = string.Empty;
    }
}