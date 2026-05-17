namespace Bus_Ticket_Booking.Mvc.Models.Booking
{
    public class CreateBookingRequestModel
    {
        public int TripId { get; set; }

        public int SeatNumber { get; set; }
    }
}