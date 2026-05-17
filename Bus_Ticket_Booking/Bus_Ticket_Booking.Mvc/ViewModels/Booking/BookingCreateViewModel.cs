namespace Bus_Ticket_Booking.Mvc.ViewModels.Booking
{
    public class BookingCreateViewModel
    {
        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public decimal Fare { get; set; }
    }
}