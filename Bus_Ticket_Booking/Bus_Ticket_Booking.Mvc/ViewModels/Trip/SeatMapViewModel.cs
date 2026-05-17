namespace Bus_Ticket_Booking.Mvc.ViewModels.Trip
{
    public class SeatMapViewModel
    {
        public int TripId { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public List<SeatViewModel> Seats
        { get; set; }
            = new();
    }
}