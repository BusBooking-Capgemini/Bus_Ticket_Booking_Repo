using API_Bus_Ticket_Booking.DTOs.Booking;

namespace TEST_Bus_Ticket_Booking.TestData
{
    public static class BookingTestData
    {
        public static CreateBookingDto GetCreateBookingDto()
        {
            return new CreateBookingDto
            {
                TripId = 1,
                SeatNumber = 12
            };
        }

        public static BookingResponseDto GetBookingResponse()
        {
            return new BookingResponseDto
            {
                BookingId = 1,
                TripId = 1,
                SeatNumber = 12,
                Status = "Booked"
            };
        }

        public static List<BookingResponseDto> GetBookings()
        {
            return new List<BookingResponseDto>
            {
                new BookingResponseDto
                {
                    BookingId = 1,
                    TripId = 1,
                    SeatNumber = 12,
                    Status = "Booked"
                },
                new BookingResponseDto
                {
                    BookingId = 2,
                    TripId = 2,
                    SeatNumber = 15,
                    Status = "Booked"
                }
            };
        }

        public static BookingDashboardDto GetDashboard()
        {
            return new BookingDashboardDto
            {
                TotalBookings = 20,
                ActiveBookings = 14
            };
        }

        public static BookingAnalyticsDto GetAnalytics()
        {
            return new BookingAnalyticsDto
            {
                OccupancyRate = 82.5,
                MostBookedTripId = 3,
                MostPopularRoute = "Delhi to Mumbai"
            };
        }
    }
}