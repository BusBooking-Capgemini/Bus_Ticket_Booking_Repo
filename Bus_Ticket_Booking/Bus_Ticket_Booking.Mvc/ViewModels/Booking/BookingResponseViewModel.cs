namespace Bus_Ticket_Booking.Mvc.ViewModels.Booking
{
    public class BookingResponseViewModel
    {
        public int BookingId { get; set; }

        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public string Status { get; set; }
            = string.Empty;
    }
}