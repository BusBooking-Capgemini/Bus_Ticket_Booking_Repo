namespace Bus_Ticket_Booking.Mvc.ViewModels.OfficePayment
{
    public class OfficePaymentViewModel
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentStatus { get; set; }
            = string.Empty;

        public DateTime PaymentDate { get; set; }

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
}