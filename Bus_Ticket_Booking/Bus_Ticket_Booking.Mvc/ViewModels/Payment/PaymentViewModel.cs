namespace Bus_Ticket_Booking.Mvc.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public int TripId { get; set; }

        public int SeatNumber { get; set; }

        public decimal Fare { get; set; }

        public string PassengerName { get; set; }
            = string.Empty;
    }
}