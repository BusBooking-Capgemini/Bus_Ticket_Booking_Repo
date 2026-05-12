namespace API_Bus_Ticket_Booking.DTOs.Booking
{
    // CREATE BOOKING
    public class CreateBookingDto
    {
        public int TripId { get; set; }

        public int SeatNumber { get; set; }
    }

    // BOOKING RESPONSE
    public class BookingResponseDto
    {
        public int BookingId { get; set; }

        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public string? Status { get; set; }
    }

    // DASHBOARD
    public class BookingDashboardDto
    {
        public int TotalBookings { get; set; }

        public int ActiveBookings { get; set; }
    }

    // ANALYTICS
    public class BookingAnalyticsDto
    {
        public double OccupancyRate { get; set; }

        public int MostBookedTripId { get; set; }

        public string MostPopularRoute { get; set; } = string.Empty;
    }
}