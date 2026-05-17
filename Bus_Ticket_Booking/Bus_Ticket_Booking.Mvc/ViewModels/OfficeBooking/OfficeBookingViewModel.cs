namespace Bus_Ticket_Booking.Mvc.ViewModels.OfficeBooking
{
    public class OfficeBookingViewModel
    {
        public int BookingId { get; set; }

        public int TripId { get; set; }

        public string FromCity { get; set; }
            = string.Empty;

        public string ToCity { get; set; }
            = string.Empty;

        public DateTime TripDate { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int SeatNumber { get; set; }

        public string Status { get; set; }
            = string.Empty;

        public decimal Fare { get; set; }

        public decimal AmountPaid { get; set; }

        public string PaymentStatus { get; set; }
            = string.Empty;
    }
}