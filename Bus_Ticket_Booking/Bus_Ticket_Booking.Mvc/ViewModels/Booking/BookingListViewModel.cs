namespace Bus_Ticket_Booking.Mvc.ViewModels.Booking
{
    public class BookingListViewModel
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
}