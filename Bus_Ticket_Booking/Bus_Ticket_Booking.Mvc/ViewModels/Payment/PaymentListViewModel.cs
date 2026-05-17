using Newtonsoft.Json;

namespace Bus_Ticket_Booking.Mvc.ViewModels.Payment
{
    public class PaymentListViewModel
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public decimal Amount { get; set; }

        [JsonProperty("paymentStatus")]
        public string Status { get; set; }
            = string.Empty;

        public DateTime PaymentDate { get; set; }
    }
}